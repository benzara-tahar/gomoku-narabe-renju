using AIEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Tic_Tac_Toe;

namespace TestProject
{


    /// <summary>
    ///Classe de test pour MinMaxSearchTest, destinée à contenir tous
    ///les tests unitaires MinMaxSearchTest
    ///</summary>
    [TestClass()]
    public class MinMaxSearchTest
    {


        /// <summary>
        ///Test pour MaxMove
        ///</summary>
        [TestMethod()]
        public void MaxMoveTest()
        {
            Board b = new Board(3, true);
            var list = new List<INode>(b.GetSccessors());
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetValue(i);
            }

            int expected = list.Count - 1;


            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test pour Search
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            Func<INode, int> f = null; // TODO: initialisez à une valeur appropriée

            INode node = null; // TODO: initialisez à une valeur appropriée
            int mode = 0; // TODO: initialisez à une valeur appropriée
            int depth = 0; // TODO: initialisez à une valeur appropriée
            int expected = 0; // TODO: initialisez à une valeur appropriée
            int actual;
            //actual = target.Search(node, mode, depth);
            //   Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Vérifiez l\'exactitude de cette méthode de test.");
        }
    }
}
