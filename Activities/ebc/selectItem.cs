using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class SelectItem<T>
    {
        private int idx = 0;
        private bool isSetIndex = false;
        private IList<T> ElementList = null;
        private bool isSetList = false;

        public void in_Index(int Idx)
        {
            this.idx = Idx;
            this.isSetIndex = true;
            MessageCompleteCheck();
        }
        public void in_Elements(IList<T> List)
        {
            this.ElementList = List;
            this.isSetList = true;
            MessageCompleteCheck();
        }
        public event Action<T> out_selectedItem;

        private void MessageCompleteCheck()
        {
            if ((isSetIndex == true) && (isSetList == true))
            {
                // Index per Modulo Verfahren in den richtigen Bereich bringen,
                // wenn der Index außerhalb des erlaubten Bereichs ist.
                if ( this.ElementList.Count <= this.idx )
                {
                    this.idx = this.idx % this.ElementList.Count;
                }

                if ((this.idx >= 0) && (this.idx < this.ElementList.Count))
                {
                    this.isSetIndex = false;
                    this.isSetList = false;
                    var Msg = this.ElementList[this.idx];
                    this.out_selectedItem(Msg);
                }
            }
        }
    }
}
