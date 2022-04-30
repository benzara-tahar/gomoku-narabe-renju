using System.Collections.Generic;

namespace AIEngine
{
    public interface INode
    {
        int GetValue();

        void SetValue(int value);

        IEnumerable<INode> GetSccessors();

        void ClearSuccessors();

        bool IsTerminal();
    }
}
