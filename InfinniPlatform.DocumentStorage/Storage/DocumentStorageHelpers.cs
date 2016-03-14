using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Interceptors;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal static class DocumentStorageHelpers
    {
        public const string AnonymousUser = "anonymous";


        public static void ExecuteCommand<TCommand>(
            this IDocumentStorageInterceptor interceptor,
            TCommand command,
            Action<TCommand> commandAction,
            Func<TCommand, DocumentStorageWriteResult<object>> onBeforeCommandAction,
            Action<TCommand, DocumentStorageWriteResult<object>, Exception> onAfterCommandAction)
        {
            ExecuteCommand(
                interceptor,
                command,
                c =>
                {
                    commandAction(c);
                    return null;
                },
                onBeforeCommandAction,
                onAfterCommandAction);
        }

        public static Task ExecuteCommandAsync<TCommand>(
            this IDocumentStorageInterceptor interceptor,
            TCommand command,
            Func<TCommand, Task> commandAction,
            Func<TCommand, DocumentStorageWriteResult<object>> onBeforeCommandAction,
            Action<TCommand, DocumentStorageWriteResult<object>, Exception> onAfterCommandAction)
        {
            return ExecuteCommandAsync(
                interceptor,
                command,
                async c =>
                      {
                          await commandAction(c);
                          return null;
                      },
                onBeforeCommandAction,
                onAfterCommandAction);
        }


        public static TResult ExecuteCommand<TCommand, TResult>(
            this IDocumentStorageInterceptor interceptor,
            TCommand command,
            Func<TCommand, TResult> commandAction,
            Func<TCommand, DocumentStorageWriteResult<TResult>> onBeforeCommandAction,
            Action<TCommand, DocumentStorageWriteResult<TResult>, Exception> onAfterCommandAction)
        {
            TResult result;

            if (interceptor == null)
            {
                result = commandAction(command);
            }
            else
            {
                var writeResult = onBeforeCommandAction(command);

                if (writeResult == null)
                {
                    writeResult = new DocumentStorageWriteResult<TResult>();
                }
                else if (!writeResult.Success)
                {
                    throw new DocumentStorageWriteException(interceptor.DocumentType, writeResult);
                }

                try
                {
                    result = commandAction(command);
                    onAfterCommandAction(command, writeResult, null);
                }
                catch (Exception exception)
                {
                    onAfterCommandAction(command, writeResult, exception);
                    throw;
                }

                if (!writeResult.Success)
                {
                    throw new DocumentStorageWriteException(interceptor.DocumentType, writeResult);
                }
            }

            return result;
        }

        public static async Task<TResult> ExecuteCommandAsync<TCommand, TResult>(
            this IDocumentStorageInterceptor interceptor,
            TCommand command,
            Func<TCommand, Task<TResult>> commandAction,
            Func<TCommand, DocumentStorageWriteResult<TResult>> onBeforeCommandAction,
            Action<TCommand, DocumentStorageWriteResult<TResult>, Exception> onAfterCommandAction)
        {
            TResult result;

            if (interceptor == null)
            {
                result = await commandAction(command);
            }
            else
            {
                var writeResult = onBeforeCommandAction(command);

                if (writeResult == null)
                {
                    writeResult = new DocumentStorageWriteResult<TResult>();
                }
                else if (!writeResult.Success)
                {
                    throw new DocumentStorageWriteException(interceptor.DocumentType, writeResult);
                }

                try
                {
                    result = await commandAction(command);
                    onAfterCommandAction(command, writeResult, null);
                }
                catch (Exception exception)
                {
                    onAfterCommandAction(command, writeResult, exception);
                    throw;
                }

                if (!writeResult.Success)
                {
                    throw new DocumentStorageWriteException(interceptor.DocumentType, writeResult);
                }
            }

            return result;
        }
    }
}