using Menekulj.Model;
using Menekulj.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ViewModelCell : ViewModelBase
    {

        public DelegateCommand? StepCommand { get; set; }

        private Int32 cellType;

        public Int32 CellType { get { return (int)cellType; } set { cellType = value; OnPropertyChanged();} }

   

        public int Row { get; private set; }
        public int Col { get;private  set; } 

        public int Id { get; private  set; }

        public ViewModelCell(int row, int col,int id )
        {
            this.Row = row;
            this.Col = col;
            this.Id = id;
        }

    }
}
