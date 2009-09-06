// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;

namespace ICSharpCode.AvalonEdit.Editing
{
	/// <summary>
	/// Determines whether the document can be modified.
	/// </summary>
	public interface IReadOnlySectionProvider
	{
		/// <summary>
		/// Gets whether insertion is possible at the specified offset.
		/// </summary>
		bool CanInsert(int offset);
		
		/// <summary>
		/// Gets the deletable segments inside the given segment.
		/// </summary>
		/// <remarks>
		/// All segments in the result must be within the given segment, and they must be returned in order
		/// (e.g. if two segments are returned, EndOffset of first segment must be less than StartOffset of second segment).
		/// </remarks>
		IEnumerable<ISegment> GetDeletableSegments(ISegment segment);
	}
}