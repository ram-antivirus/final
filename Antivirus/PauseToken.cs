// Decompiled with JetBrains decompiler
// Type: MSNetwork4Demo2.PauseToken
// Assembly: WpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08F43AB3-80A9-45A9-8890-4728DE8B53D4
// Assembly location: C:\Users\sahil\Documents\Visual Studio 2012\Projects\WpfApplication1\WpfApplication1\bin\Debug\WpfApplication1.exe

using System.Threading.Tasks;

namespace MSNetwork4Demo2
{
  public struct PauseToken
  {
    private readonly PauseTokenSource m_source;

    public bool IsPaused
    {
      get
      {
        return this.m_source != null && this.m_source.IsPaused;
      }
    }

    internal PauseToken(PauseTokenSource source)
    {
      this.m_source = source;
    }

    public Task WaitWhilePausedAsync()
    {
      return this.IsPaused ? this.m_source.WaitWhilePausedAsync() : PauseTokenSource.s_completedTask;
    }
  }
}
