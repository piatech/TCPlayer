// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TCPlayer
{
    class MostRecentProjects : IEnumerable<Tuple<string, string>>
    {
        private class Project
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public Project(string Name, string FilePath)
            {
                this.Name = Name;
                this.FilePath = Path.GetFullPath(FilePath);
            }
        }

        private List<Project> _projects = new List<Project>();
        private RegistryKey _appKey;
        private bool _changesDetected = false;

        public MostRecentProjects(RegistryKey appKey)
        {
            _appKey = appKey;

            RegistryKey recentProjectsKey = appKey.OpenSubKey("RecentProjects");

            if(recentProjectsKey == null)
            {
                return;
            }

            try 
            {
                // Number of recent projects
                int count = int.Parse((string)recentProjectsKey.GetValue("Count"));

                for(int i = 0; i < count; i++)
                {
                    string filePath = (string)recentProjectsKey.GetValue(string.Format("Path{0}", i));

                    if(!File.Exists(filePath))
                    {
                        _changesDetected = true;
                        continue;
                    }

                    _projects.Add(new Project((string)recentProjectsKey.GetValue(string.Format("Project{0}", i)),
                                              filePath));
                }
            }
            catch(Exception)
            {
                return;
            }
        }

        internal void AddProject(string ProjectName, string ProjectFilePath)
        {
            ProjectFilePath = Path.GetFullPath(ProjectFilePath);

            // Check if the project already saved in MRP
            foreach(Project project in _projects)
            {
                // Break if the path is already saved in the list of MRP
                if (project.FilePath == ProjectFilePath)
                {
                    return;
                }
            }

            // Add the new project
            _projects.Add(new Project(ProjectName, ProjectFilePath));

            _changesDetected = true;
        }

        internal void Save()
        {
            if (!_changesDetected)
            {
                return;
            }

            RegistryKey recentProjectsKey = _appKey.OpenSubKey("RecentProjects");

            if(recentProjectsKey != null)
            {
                _appKey.DeleteSubKeyTree("RecentProjects");
            }

            recentProjectsKey = _appKey.CreateSubKey("RecentProjects");

            int i = 0;

            foreach (Project project in _projects)
            {
                if(string.IsNullOrEmpty(project.Name) || string.IsNullOrEmpty(project.FilePath))
                {
                    continue;
                }

                recentProjectsKey.SetValue(string.Format("Project{0}", i), project.Name);
                recentProjectsKey.SetValue(string.Format("Path{0}", i), project.FilePath);
                i++;
            }

            recentProjectsKey.SetValue("Count", i.ToString());
        }

        internal void ClearAll()
        {
            RegistryKey recentProjectsKey = _appKey.OpenSubKey("RecentProjects");

            if (recentProjectsKey != null)
            {
                _appKey.DeleteSubKeyTree("RecentProjects");
            }
        }

        public IEnumerator<Tuple<string, string>> GetEnumerator()
        {
            foreach(Project project in _projects)
            {
                yield return new Tuple<string, string>(project.Name, project.FilePath);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
