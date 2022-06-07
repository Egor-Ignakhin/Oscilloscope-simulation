using NUnit.Framework;

using OscilloscopeSimulation.Editor;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

namespace OscilloscopeSimulation.Tests
{
    internal sealed class StdSceneFoldersCreatorTests
    {
        [UnityTest]
        public IEnumerator CreateStdFoldersTest()
        {
            Assert.IsNotNull(StdSceneFoldersCreator.Folders);
            Assert.IsNotEmpty(StdSceneFoldersCreator.Folders);

            StdSceneFoldersCreator.CreateStdFolders();

            Assert.IsTrue(StdFolderWasCreated());

            return null;
        }
        private bool StdFolderWasCreated()
        {
            foreach (var folder in StdSceneFoldersCreator.Folders)
            {
                if (!GameObject.Find(folder))
                    return false;
            }

            return true;
        }
    }
}