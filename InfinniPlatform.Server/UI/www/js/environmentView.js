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