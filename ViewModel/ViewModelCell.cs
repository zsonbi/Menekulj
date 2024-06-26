﻿using Menekulj.Model;
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

        private Cell cellType;

        public Cell CellType { get { return cellType; } set { cellType = (Cell)value; OnPropertyChanged();} }

   

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
