using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.CSharp;

namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
	/// <summary>
	/// Компилятор прикладных скриптов, написанных на C#.
	/// </summary>
	sealed class ScriptCompilerCSharp : IScriptCompiler
	{
		/// <summary>
		/// Компилировать скрипт.
		/// </summary>
		/// <param name="scripts">Список метаданных скриптов.</param>
		public IEnumerable<Script> Compile(IEnumerable scripts)
		{
			if (scripts.IsNullOrEmpty() == false)
			{
				return BuildDelegates(scripts);
			}

			return null;
		}


		private static IEnumerable<Script> BuildDelegates(IEnumerable scripts)
		{
			var result = new List<Script>();

			var scriptsAssembly = BuildAssembly(scripts);
			var scriptsType = scriptsAssembly.GetType("Scripts");

			foreach (dynamic script in scripts)
			{
				string scriptName = script.Name;

				var scriptMethod = scriptsType.GetMethod(scriptName);

				if (scriptMethod != null)
				{
					result.Add(new Script
							   {
								   Name = scriptName,
								   Action = (context, arguments) => InvokeScript(scriptMethod, context, arguments)
							   });
				}
			}

			return result;
		}

		private static Assembly BuildAssembly(IEnumerable scripts)
		{
			var provider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });

			var compilerParameters = new CompilerParameters
									 {
										 GenerateInMemory = true,
										 GenerateExecutable = false,
										 CompilerOptions = "/optimize",
									 };

			// Add references

			var executingAssembly = Assembly.GetExecutingAssembly();

			var referencedAssemblies = new[] { executingAssembly }
				.Concat(executingAssembly.GetReferencedAssemblies().Select(Assembly.Load))
				.Select(a => a.Location)
				.ToArray();

			compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies);

			// Generate scripts code
			var scriptsCode = BuildCode(scripts);

			// Compile scripts code
			var results = provider.CompileAssemblyFromSource(compilerParameters, scriptsCode);

			if (results.Errors.HasErrors)
			{
				var errors = new StringBuilder("Compiler Errors:").AppendLine();

				foreach (CompilerError error in results.Errors)
				{
					errors.AppendFormat("Line: {0}, Column: {1}, Error: {2}", error.Line, error.Column, error.ErrorText).AppendLine();
				}

				throw new InvalidOperationException(errors.ToString());
			}

			return results.CompiledAssembly;
		}

		private static string BuildCode(IEnumerable scripts)
		{
			var builder = new StringBuilder();

			// Add usings
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Linq;");
			builder.AppendLine("using System.Collections;");
			builder.AppendLine("using System.Collections.Generic;");

			// Add script class
			builder.AppendLine("public static class Scripts {");

			// Add script methods
			foreach (dynamic script in scripts)
			{
				builder.AppendFormat("public static void {0}(dynamic context, dynamic arguments) {{ {1} }}", script.Name, script.Body).AppendLine();
			}

			builder.AppendLine("}");

			return builder.ToString();
		}

		private static void InvokeScript(MethodInfo scriptMethod, dynamic context, dynamic arguments)
		{
			try
			{
				scriptMethod.Invoke(null, new object[] { context, arguments });
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
		}
	}
}