﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using ICSharpCode.SharpDevelop.Project;
using SD = ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.PackageManagement.EnvDTE
{
	public class Project
	{
		IPackageManagementProjectService projectService;
		IPackageManagementFileService fileService;
		DTE dte;
		
		public Project(MSBuildBasedProject project)
			: this(
				project,
				new PackageManagementProjectService(),
				new PackageManagementFileService())
		{
		}
		
		public Project(
			MSBuildBasedProject project,
			IPackageManagementProjectService projectService,
			IPackageManagementFileService fileService)
		{
			this.MSBuildProject = project;
			this.projectService = projectService;
			this.fileService = fileService;
			
			CreateProperties();
			Object = new ProjectObject(this);
			ProjectItems = new ProjectItems(this, fileService);
		}
		
		void CreateProperties()
		{
			var propertyFactory = new ProjectPropertyFactory(this);
			Properties = new Properties(propertyFactory);
		}
		
		public string Name {
			get { return MSBuildProject.Name; }
		}
		
		public ProjectObject Object { get; private set; }
		public Properties Properties { get; private set; }
		public ProjectItems ProjectItems { get; private set; }
		
		public DTE DTE {
			get {
				if (dte == null) {
					dte = new DTE(projectService, fileService);
				}
				return dte;
			}
		}
		
		internal MSBuildBasedProject MSBuildProject { get; private set; }
		
		internal void Save()
		{
			projectService.Save(MSBuildProject);
		}
		
		internal void AddReference(string path)
		{
			if (!HasReference(path)) {
				var referenceItem = new ReferenceProjectItem(MSBuildProject, path);
				projectService.AddProjectItem(MSBuildProject, referenceItem);
			}
		}
		
		bool HasReference(string include)
		{
			foreach (ReferenceProjectItem reference in GetReferences()) {
				if (IsReferenceMatch(reference, include)) {
					return true;
				}
			}
			return false;
		}
		
		internal IEnumerable<SD.ProjectItem> GetReferences()
		{
			return MSBuildProject.GetItemsOfType(ItemType.Reference);
		}
		
		bool IsReferenceMatch(ReferenceProjectItem reference, string include)
		{
			return String.Equals(reference.Include, include, StringComparison.InvariantCultureIgnoreCase);
		}
		
		internal void RemoveReference(ReferenceProjectItem referenceItem)
		{
			projectService.RemoveProjectItem(MSBuildProject, referenceItem);
		}
		
		internal void AddFile(string include)
		{
			var fileProjectItem = CreateFileProjectItem(include);
			projectService.AddProjectItem(MSBuildProject, fileProjectItem);
		}
		
		FileProjectItem CreateFileProjectItem(string include)
		{
			ItemType itemType = GetDefaultItemType(include);
			return CreateFileProjectItem(itemType, include);
		}
		
		ItemType GetDefaultItemType(string include)
		{
			return MSBuildProject.GetDefaultItemType(include);
		}
		
		FileProjectItem CreateFileProjectItem(ItemType itemType, string include)
		{
			var fileProjectItem = new FileProjectItem(MSBuildProject, itemType);
			fileProjectItem.Include = include;
			return fileProjectItem;
		}
	}
}