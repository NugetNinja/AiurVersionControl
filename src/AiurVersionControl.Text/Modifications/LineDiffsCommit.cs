﻿using AiurVersionControl.CRUD;
using AiurVersionControl.Models;
using NetDiff;
using System;
using System.Collections.Generic;

namespace AiurVersionControl.Text.Modifications
{
    public class LineDiffsCommit : IModification<CollectionWorkSpace<string>>
    {
        public LineDiff[] Diff { get; set; }

        [Obsolete(error: true, message: "This method is for Newtonsoft.Json")]
        public LineDiffsCommit() { }

        public LineDiffsCommit(DiffResult<string>[] sourceDiff)
        {
            var i = 0;
            var lineDiffs = new List<LineDiff>();
            foreach (var diffItem in sourceDiff)
            {
                switch (diffItem.Status)
                {
                    case DiffStatus.Equal:
                        i++;
                        break;
                    case DiffStatus.Inserted:
                        lineDiffs.Add(new LineDiff
                        {
                            LineNumber = i,
                            Status = DiffStatus.Inserted,
                            NewLine = diffItem.Obj2
                        });
                        i++;
                        break;
                    case DiffStatus.Deleted:
                        lineDiffs.Add(new LineDiff
                        {
                            LineNumber = i,
                            Status = DiffStatus.Deleted
                        });
                        break;
                }
            }
            Diff = lineDiffs.ToArray();
        }

        public void Apply(CollectionWorkSpace<string> workspace)
        {
            foreach (var diffItem in Diff)
            {
                switch (diffItem.Status)
                {
                    case DiffStatus.Inserted:
                        workspace.List.Insert(diffItem.LineNumber, diffItem.NewLine);
                        break;
                    case DiffStatus.Deleted:
                        workspace.List.RemoveAt(diffItem.LineNumber);
                        break;
                }
            }
        }
    }
}
