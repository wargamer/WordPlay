namespace WordPlay.WordDSL
{
    using System;
    using System.Collections.Generic;
    using HtmlAgilityPack;

    internal static class HTMLHelper
    {
        /// <summary>
        /// Returns true if node contains a class property with value of classname
        /// </summary>
        /// <param name="node"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        private static bool IsOfClass(this HtmlNode node, string classname)
        {
            return (node.GetAttributeValue("class", string.Empty).Equals(classname, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Retrieves all nodes with the given Class property beneath the rootnode
        /// </summary>
        /// <param name="rootnode"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByClass(this HtmlNode rootnode, string classname)
        {
            if (rootnode.IsOfClass(classname))
                yield return rootnode;
            foreach (var node in rootnode.ChildNodes)
            {   
                foreach (var childnode in GetNodesByClass(node, classname))
                    yield return childnode;
            }
        }
    }
}
