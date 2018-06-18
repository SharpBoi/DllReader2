using DllReader2.AssemblyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DllReader2
{
    public class AssembliesVisualizer
    {
        #region Fields
        private TreeView asmTree;
        private TreeView asmContentTree;

        private DescriptionMode componentsDescriptionMode;
        #endregion

        #region Funcs
        public AssembliesVisualizer(TreeView AsmTree, TreeView AsmContentTree)
        {
            this.asmTree = AsmTree;
            asmTree.SelectedItemChanged += AsmTree_SelectedItemChanged;

            this.asmContentTree = AsmContentTree;
        }

        private void AsmTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                if (e.NewValue is TreeViewItem)
                {
                    clearContentTree();
                    showDllContent((e.NewValue as TreeViewItem).Header as AssemblyDllData);
                }
            }
        }

        public void ShowAssemblies(List<AssemblyDllData> dlls)
        {
            for (int i = 0; i < dlls.Count; i++)
            {
                asmTree.Items.Add(new TreeViewItem()
                {
                    Foreground = Brushes.White,
                    FontSize = 20,
                    Header = dlls[i]
                });
            }
        }
        public void Clear()
        {
            clearAssembliesTree();
            clearContentTree();

            GC.Collect();
        }

        private void showDllContent(AssemblyDllData dll)
        {
            for (int i = 0; i < dll.Types.Length; i++)
            {
                if (dll.Types[i].TypeKeyWord == TypeKeyWords.Class)
                    addTypeContentToTree(dll.Types[i]);
            }
        }
        private void addTypeContentToTree(AssemblyTypeData type)
        {
            type.TypeDescriptionMode = componentsDescriptionMode;

            if (type.TypeKeyWord == TypeKeyWords.Class)
            {
                TreeViewItem typeItem = createItem(type);
                for (int i = 0; i < type.DeclaredMethods.Length; i++)
                {
                    type.DeclaredMethods[i].MethodDescriptionMode = componentsDescriptionMode;

                    if (type.DeclaredMethods[i].AccessModifier.HasFlag(MethodAttributes.Public)
                        || type.DeclaredMethods[i].AccessModifier.HasFlag(MethodAttributes.Family))
                    {
                        TreeViewItem methodItem = createItem(type.DeclaredMethods[i]);
                        typeItem.Items.Add(methodItem);
                    }
                }

                asmContentTree.Items.Add(typeItem);
            }
        }
        private void changeConentDescription(DescriptionMode descriptionMode)
        {
            componentsDescriptionMode = descriptionMode;

            for (int i = 0; i < asmContentTree.Items.Count; i++)
            {
                TreeViewItem typeItem = asmContentTree.Items[i] as TreeViewItem;
                if (typeItem.Header is AssemblyTypeData)
                {
                    AssemblyTypeData type = typeItem.Header as AssemblyTypeData;
                    type.TypeDescriptionMode = descriptionMode;

                    typeItem.Header = null;
                    typeItem.Header = type;
                }

                for (int j = 0; j < typeItem.Items.Count; j++)
                {
                    TreeViewItem methodItem = typeItem.Items[j] as TreeViewItem;
                    if (methodItem.Header is AssemblyMethodData)
                    {
                        AssemblyMethodData method = methodItem.Header as AssemblyMethodData;
                        method.MethodDescriptionMode = descriptionMode;

                        methodItem.Header = null;
                        methodItem.Header = method;
                    }
                }
            }
        }

        private TreeViewItem createItem(object header)
        {
            TreeViewItem item = new TreeViewItem();

            item.Header = header;
            item.Foreground = Brushes.White;
            item.FontSize = 20;

            return item;
        }
        
        private void clearAssembliesTree()
        {
            asmTree.Items.Clear();
        }
        private void clearContentTree()
        {
            asmContentTree.Items.Clear();
        }

        #endregion

        #region Prop
        public DescriptionMode ContentDescriptionMode
        {
            get { return componentsDescriptionMode; }
            set
            {
                componentsDescriptionMode = value;
                changeConentDescription(value);
            }
        }
        #endregion
    }
}
