using System;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Graph;
using Dynamo.Models;
using Dynamo.UI.Commands;
using Dynamo.ViewModels;

namespace Collaborate
{
    public class CollaborateWindowViewModel : NotificationObject, IDisposable
    {
        private string activeNodeTypes;
        private ReadyParams readyParams;

        // Displays active nodes in the workspace
        public string ActiveNodeTypes
        {
            get
            {
                activeNodeTypes = getNodeTypes();
                return activeNodeTypes;
            }
        }

        // Helper function that builds string of active nodes
        public string getNodeTypes()
        {
            string output = "Active nodes:\n";

            foreach (NodeModel node in readyParams.CurrentWorkspaceModel.Nodes)
            {
                string nickName = node.Name;
                output += nickName + "\n";
            }

            return output;
        }

        public CollaborateWindowViewModel(ReadyParams p, DynamoViewModel vm)
        {
            // This constructor is triggered every time the menu button is clicked.

            readyParams = p;
            DynamoViewModel viewModel = vm;

            // Read directory contents
            // var graphs = System.IO.Directory.EnumerateFiles(directoryPath);
            // We'll need a counter here because the above enumeration doesn't have a Count property
            // and we'll want to let the user know how many fiels we have processed in the end.

            string referenceFile = "C:\\Users\\elsdonl0213\\Repos\\Collaborate\\Resources\\example-2.0.2-v2.dyn";

            var graphCount = 0;
            // Cycle through all files found in the directory
            var ext = System.IO.Path.GetExtension(referenceFile);
            // We're only interested in *.dyn files
            if (ext == ".dyn")
            {
                // Open the graph
                viewModel.OpenCommand.Execute(referenceFile);
                // Set the graph run type to manual mode (otherwise some graphs might auto-execute at this point)
                viewModel.CurrentSpaceViewModel.RunSettingsViewModel.Model.RunType = RunType.Manual;
                // Call our main method
                //UnfancifyGraph();
                // Save the graph
                //viewModel.SaveAsCommand.Execute(graph);
                // Close it
                //viewModel.CloseHomeWorkspaceCommand.Execute(null);
                // Increment our counter
                graphCount += 1;
                // Update the message in the UI
                //UnfancifyMsg += "Unfancified " + graph + "\n";
            }


            p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodesChanged;
            p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodesChanged;
        }

        private void CurrentWorkspaceModel_NodesChanged(NodeModel obj)
        {
            RaisePropertyChanged("ActiveNodeTypes");
        }

        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodesChanged;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodesChanged;
        }
    }
}
