/** Форматирует список переменных окружения для отображения в DataGrid * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function FormatResult(context, args) {
  var variables = [];

  for (var property in args.value.Result) {
    variables.push({
      Name: property,
      Value: args.value.Result[property]
    });
  }

  return variables;
}