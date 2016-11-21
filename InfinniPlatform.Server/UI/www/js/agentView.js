function SetSelectedItem(context, args) {
  var agent = context.parameters.Agent.getValue();

  if (agent) {
    context.dataSources.AgentsDataSource.setSelectedItem(agent);    
  }
}