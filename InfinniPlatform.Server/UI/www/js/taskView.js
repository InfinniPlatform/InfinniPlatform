/** Обновляет информацию о выполнении задачи * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function RefreshTaskStatus(context, args) {
  var value = args.data.Result;

  context.parameters.Task.setValue(value);

  toastr.success('Refreshed.');
}