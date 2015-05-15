using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace InfinniPlatform.MetadataDesigner.Views.Status
{
	public sealed class StatusProcess
	{
		private EventWaitHandle _waitHandle = new EventWaitHandle(true,EventResetMode.AutoReset);

		public void StartOperation(Action operation)
		{
			var form = new StatusForm();

			_waitHandle.WaitOne();
			ThreadPool.QueueUserWorkItem(data =>
											 {
												 operation.Invoke();
												 form.ActionToInvoke = new Action(() => form.DialogResult = DialogResult.OK);											 
												 _waitHandle.Set();
											 });
			var timer = new Timer();
			timer.Interval = 1000;
			timer.Tick += (sender, obj) =>
							  {
								  using (var fileStream = new FileStream("console.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
								  {
									  using (var textReader = new StreamReader(fileStream))
									  {
										  var lines = textReader.ReadToEnd().Split(new[] {'\n'}).Reverse().Take(500).Reverse();
										  form.LogMemo.Text = string.Join("\n",lines);
										  form.LogMemo.SelectionStart = form.LogMemo.Text.Length;
										  form.LogMemo.ScrollToCaret();
									  }
								  }
							  };
			timer.Enabled = true;

			form.ShowDialog();
		}

		public void EndOperation()
		{
			_waitHandle.WaitOne();
		}
	}
}
