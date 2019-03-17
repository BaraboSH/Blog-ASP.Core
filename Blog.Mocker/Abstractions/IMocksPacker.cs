using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Mocker;

namespace Blog.Mocker.Abstractions
{
    public interface IMocksPacker
    {
         Task<Pack> GetPack(List<string> usernames);
    }
}