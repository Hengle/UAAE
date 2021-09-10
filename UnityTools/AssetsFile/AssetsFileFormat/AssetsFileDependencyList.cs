using System.Collections.Generic;

namespace UnityTools
{
    public class AssetsFileDependencyList
    {
        public int dependencyCount;
        public List<AssetsFileDependency> dependencies;
        public void Read(AssetsFileReader reader)
        {
            dependencyCount = reader.ReadInt32();
            dependencies = new List<AssetsFileDependency>();
            for (var i = 0; i < dependencyCount; i++)
            {
                var dependency = new AssetsFileDependency();
                dependency.Read(reader);
                dependencies.Add(dependency);
            }
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(dependencyCount);
            for (var i = 0; i < dependencyCount; i++)
            {
                dependencies[i].Write(writer);
            }
        }
    }
}
