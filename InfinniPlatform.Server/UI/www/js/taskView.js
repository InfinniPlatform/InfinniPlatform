function RefreshTaskStatus(context, args) {
  var value = args.data.Result;
  context.parameters.Task.setValue(value);
  toastr.success('Refreshed.');
}