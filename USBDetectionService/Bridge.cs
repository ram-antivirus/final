using System;
using System.Diagnostics;
using System.Text;

namespace Wt.Cli
{
	internal class Bridge
	{
		string targetApp;

		 Process console;

		 bool isFree;

		public event GetMsgHandler getMsg;

		public event End end;

		public Bridge(string app)
		{
			this.targetApp = app;
			this.isFree = true;
		}

		public bool CommandAsync(string cmd)
		{
			if (this.isFree)
			{
				this.console = new Process();
				this.Config(this.console);
				this.console.StartInfo.Arguments = cmd;
				this.console.OutputDataReceived += new DataReceivedEventHandler(this.OutputMsg);
				this.console.EnableRaisingEvents = true;
				this.console.Exited += new EventHandler(this.End);
				this.console.Start();
				this.isFree = false;
				this.console.BeginOutputReadLine();
				return true;
			}
			return false;
		}

		public void Stop()
		{
			try
			{
				this.console.Kill();
				this.isFree = true;
			}
			catch (Exception)
			{
			}
		}

		public string Command(string cmd)
		{
			Process process = new Process();
			this.Config(process);
			process.StartInfo.Arguments = cmd;
			process.Start();
			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			process.Close();
			return result;
		}

		public void Config(Process console)
		{
			console.StartInfo.FileName = this.targetApp;
			console.StartInfo.CreateNoWindow = true;
			console.StartInfo.UseShellExecute = false;
			console.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			console.StartInfo.RedirectStandardOutput = true;
			console.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("cp866");
		}

		public void OutputMsg(object sendingProcess, DataReceivedEventArgs recieved)
		{
			this.getMsg(recieved.Data);
		}

		private void End(object sender, EventArgs e)
		{
			this.console.WaitForExit();
			this.console.Close();
			this.isFree = true;
			this.end();
		}
	}
}
