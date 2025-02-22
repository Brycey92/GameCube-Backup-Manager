﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using sio = System.IO;

namespace GCBM
{
    public partial class frmMain : Form
    {
        private TOCClass toc;

        private void GetFilDirInfo(sio.DirectoryInfo pDir, ref int itemNum, ref int filePos)
        {
            sio.DirectoryInfo di;
            sio.DirectoryInfo[] dirs;
            sio.FileInfo[] fils;
            TOCItemFil tif;
            var tocDirIdx = itemNum - 1;

            dirs = pDir.GetDirectories();
            for (var cnt = 0; cnt < dirs.Length; cnt++)
                if (dirs[cnt].Name.ToLower() == "&&systemdata")
                {
                    di = dirs[0];
                    dirs[0] = dirs[cnt];
                    dirs[cnt] = di;
                    break;
                }

            for (var cnt = 0; cnt < dirs.Length; cnt++)
            {
                tif = new TOCItemFil(itemNum, tocDirIdx, tocDirIdx, 0, true,
                    dirs[cnt].Name, dirs[cnt].FullName.Replace(RES_PATH, ""), dirs[cnt].FullName);
                toc.fils.Add(tif);
                itemNum += 1;
                toc.dirCount += 1;
                GetFilDirInfo(dirs[cnt], ref itemNum, ref filePos);
            }

            fils = pDir.GetFiles();
            for (var cnt = 0; cnt < fils.Length; cnt++)
            {
                tif = new TOCItemFil(itemNum, tocDirIdx, filePos, (int)fils[cnt].Length, false,
                    fils[cnt].Name, fils[cnt].FullName.Replace(RES_PATH, ""), fils[cnt].FullName);
                toc.fils.Add(tif);
                toc.fils[0].len = toc.fils.Count;
                filePos += 2;
                itemNum += 1;
                toc.filCount += 1;
            }

            toc.fils[tocDirIdx].len = itemNum;
        }

        private bool GenerateTreeView(bool fileNameSort)
        {
            var tns = new List<TreeNode>();
            TreeNode tn, tnn;
            int idx;
            int j;

            tn = new TreeNode(toc.fils[0].name, 0, 0);
            tn.Name = toc.fils[0].TOCIdx.ToString();
            tn.ToolTipText = RES_PATH;
            toc.fils[0].node = tn;
            tns.Add(tn);

            if (fileNameSort)
            {
                for (var i = 1; i < toc.fils.Count; i++)
                    if (toc.fils[i].isDir)
                    {
                        for (j = 0; j < tns.Count; j++)
                            if (tns[j].Name == toc.fils[i].dirIdx.ToString())
                                break;
                        if (j == tns.Count)
                        {
                            MessageBox.Show("GenerateTreeView() error: dir2dir not found", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        tn = tns[j];
                        tnn = new TreeNode(toc.fils[i].name, 1, 2);
                        tnn.Name = toc.fils[i].TOCIdx.ToString();
                        tnn.ToolTipText = toc.fils[i].path;
                        tnn.Tag = i;
                        toc.fils[i].node = tnn;
                        tns.Add(tnn);
                        tn.Nodes.Add(tnn);
                    }
                    else
                    {
                        for (j = 0; j < tns.Count; j++)
                            if (tns[j].Name == toc.fils[i].dirIdx.ToString())
                                break;
                        if (j == tns.Count)
                        {
                            MessageBox.Show("GenerateTreeView() error: dir2fil not found", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        tn = tns[j];
                        tnn = new TreeNode(toc.fils[i].name, 3, 3);
                        tnn.Name = toc.fils[i].TOCIdx.ToString();
                        tnn.ToolTipText = toc.fils[i].path;
                        tnn.Tag = i;
                        toc.fils[i].node = tnn;
                        tn.Nodes.Add(tnn);
                    }
            }
            else
            {
                idx = 2;
                for (var i = 1; i < toc.fils.Count; i++)
                    if (!toc.fils[i].isDir)
                    {
                        tnn = new TreeNode(toc.fils[idx].gamePath, 3, 3);
                        tnn.Name = toc.fils[idx].TOCIdx.ToString();
                        tnn.ToolTipText = toc.fils[idx].path;
                        tnn.Tag = idx;
                        toc.fils[idx].node = tnn;
                        tn.Nodes.Add(tnn);
                        if (toc.fils[idx].name == "opening.bnr")
                            idx = idx;
                        idx = toc.fils[i].nextIdx;
                    }
            }

            return true;
        }

        //private delegate void ResetProgressBarCB(int min, int max, int val);
        //private delegate void UpdateProgressBarCB(int val);
        //private delegate void UpdateActionLabelCB(string text);
        private delegate void ResetControlsCB(bool error, string errorText);

        private delegate int ModCB(int val);

        #region TOC class

        private class TOCClass : IComparer<TOCItemFil>, ICloneable
        {
            public readonly List<TOCItemFil> fils;
            public int dataStart;
            public int dirCount = 1;
            public int filCount = 4;
            public int lastIdx;
            public int startIdx;
            public int totalLen;

            public TOCClass(string resPath)
            {
                fils = new List<TOCItemFil>();
                fils.Add(new TOCItemFil(0, 0, 0, 99999, true, "root", "", resPath));
                fils.Add(new TOCItemFil(1, 0, 0, 6, true, "&&SystemData", "&&systemdata\\",
                    resPath + "&&systemdata\\"));
                fils.Add(new TOCItemFil(2, 1, 0, 99999, false, "ISO.hdr", "&&SystemData\\iso.hdr",
                    resPath + "&&SystemData\\iso.hdr"));
                fils.Add(new TOCItemFil(3, 1, 9280, 99999, false, "AppLoader.ldr", "&&SystemData\\apploader.ldr",
                    resPath + "&&SystemData\\apploader.ldr"));
                fils.Add(new TOCItemFil(4, 1, 0, 99999, false, "Start.dol", "&&SystemData\\start.dol",
                    resPath + "&&SystemData\\start.dol"));
                fils.Add(new TOCItemFil(5, 1, 0, 99999, false, "Game.toc", "&&SystemData\\game.toc",
                    resPath + "&&SystemData\\game.toc"));

                totalLen = 0;
                dataStart = totalLen;
                startIdx = totalLen;
            }

            #region ICloneable Members

            public object Clone()
            {
                TOCClass res;

                res = new TOCClass(fils[0].path);
                res.fils.Clear();
                res.dirCount = dirCount;
                res.filCount = filCount;
                for (var i = 0; i < fils.Count; i++)
                    res.fils.Add((TOCItemFil)fils[i].Clone());

                return res;
            }

            #endregion

            #region IComparer<TOCItemFil> Members

            public int Compare(TOCItemFil x, TOCItemFil y)
            {
                if (x.pos > y.pos)
                    return 1;
                if (x.pos < y.pos)
                    return -1;
                return 0;
            }

            #endregion
        }

        private class TOCItemFil : ICloneable
        {
            public readonly int dirIdx;
            public readonly string gamePath;
            public readonly bool isDir;
            public readonly string name;
            public readonly string path;
            public readonly int TOCIdx;
            public int len;
            public int nextIdx;
            public TreeNode node;
            public int pos;
            public int prevIdx;

            public TOCItemFil(int TOCIdx, int dirIdx, int pos, int len, bool isDir, string name, string gamePath,
                string path)
            {
                this.TOCIdx = TOCIdx;
                this.dirIdx = dirIdx;
                this.pos = pos;
                this.len = len;
                this.isDir = isDir;
                this.name = name;
                this.gamePath = gamePath;
                this.path = path;
            }

            #region ICloneable Members

            public object Clone()
            {
                return new TOCItemFil(TOCIdx, dirIdx, pos, len, isDir, name, gamePath, path);
            }

            #endregion
        }

        #endregion
    }
}

#region SIOExtensions

//end

#endregion