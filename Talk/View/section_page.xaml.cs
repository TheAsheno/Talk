using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.View
{
    //版块页面
    public partial class section_page : Page
    {
        SectionViewModel sectionViewModel;
        public section_page()
        {
            InitializeComponent();
            sectionViewModel = new SectionViewModel();
            DataContext = sectionViewModel;
            displaySection();
        }

        //显示版块列表
        private void displaySection()
        {
            for (int i = 0; i < sectionViewModel.sectionModel.Sections.Count(); i++)
            {
                // 创建 Border
                Border border = new Border();
                border.Margin = new Thickness(10); // 设置边距
                border.BorderThickness = new Thickness(2); // 设置边框厚度
                border.BorderBrush = Brushes.Black; // 设置边框颜色

                // 创建 Grid
                Grid grid = new Grid();
                grid.Height = 100;
                grid.Background = Brushes.White;

                // 添加列定义
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });

                // 在第一列添加 StackPanel
                StackPanel stackPanel = new StackPanel();
                Grid.SetColumn(stackPanel, 0);

                // 添加 TextBlock 到 StackPanel
                TextBlock titleTextBlock = new TextBlock();
                titleTextBlock.Text = sectionViewModel.sectionModel.Sections[i].Name;
                titleTextBlock.Tag = sectionViewModel.sectionModel.Sections[i].Sid;
                titleTextBlock.Cursor = Cursors.Hand;
                titleTextBlock.FontSize = 18;
                titleTextBlock.FontWeight = FontWeights.Bold;
                titleTextBlock.Foreground = Brushes.Black;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                titleTextBlock.Margin = new Thickness(0, 20, 0, 5);
                stackPanel.Children.Add(titleTextBlock);
                //点击版块名跳转到其对应的帖子列表页面
                titleTextBlock.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_postlist(clickedTextBlock.Tag.ToString(), clickedTextBlock.Text);
                };

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Style = FindResource("textHint") as Style; // 根据需要找到资源
                textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.Children.Add(textBlock1);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = "版主 " + sectionViewModel.sectionModel.Sections[i].MasterName;
                textBlock2.Style = FindResource("textHint") as Style; // 根据需要找到资源
                textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock2.Margin = new Thickness(10);
                textBlock2.FontSize = 10;
                stackPanel.Children.Add(textBlock2);

                grid.Children.Add(stackPanel);

                // 在第二列添加 Grid
                Grid grid2 = new Grid();
                Grid.SetColumn(grid2, 1);
                grid2.Margin = new Thickness(15, 0, 0, 0);

                // 添加 TextBlock 和其他控件到 Grid
                TextBlock arrowTextBlock = new TextBlock();
                arrowTextBlock.Text = ">>>";
                arrowTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                arrowTextBlock.VerticalAlignment = VerticalAlignment.Top;
                arrowTextBlock.Margin = new Thickness(0, 10, 0, 0);
                grid2.Children.Add(arrowTextBlock);

                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = sectionViewModel.sectionModel.Sections[i].Statement;
                textBlock3.FontSize = 12;
                textBlock3.MaxHeight = 50;
                textBlock3.Foreground = Brushes.Black;
                textBlock3.TextAlignment = TextAlignment.Left;
                textBlock3.Margin = new Thickness(0, 30, 15, 20);
                textBlock3.TextWrapping = TextWrapping.Wrap;
                grid2.Children.Add(textBlock3);

                Grid bottomGrid = new Grid();
                bottomGrid.VerticalAlignment = VerticalAlignment.Bottom;
                bottomGrid.HorizontalAlignment = HorizontalAlignment.Right;
                bottomGrid.Margin = new Thickness(0, 5, 0, 5);

                // 添加列定义
                bottomGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
                bottomGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });

                TextBlock textBlock4 = new TextBlock();
                Grid.SetColumn(textBlock4, 0);
                textBlock4.Text = sectionViewModel.sectionModel.Sections[i].ClickCount.ToString() + " click";
                textBlock4.Style = FindResource("textHint") as Style; // 根据需要找到资源
                textBlock4.FontSize = 10;
                textBlock4.HorizontalAlignment = HorizontalAlignment.Center;
                bottomGrid.Children.Add(textBlock4);

                TextBlock textBlock5 = new TextBlock();
                Grid.SetColumn(textBlock5, 1);
                textBlock5.Text = sectionViewModel.sectionModel.Sections[i].PostCount.ToString() + " post";
                textBlock5.Style = FindResource("textHint") as Style; // 根据需要找到资源
                textBlock5.FontSize = 10;
                textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
                bottomGrid.Children.Add(textBlock5);

                grid2.Children.Add(bottomGrid);
                grid.Children.Add(grid2);

                // 在 Grid 中添加一条线
                Line line = new Line();
                line.X1 = 118;
                line.Y1 = 10;
                line.X2 = 118;
                line.Y2 = 90;
                line.StrokeThickness = 2;
                line.Stroke = Brushes.Black;
                grid.Children.Add(line);

                // 将 Grid 添加到 Border 中
                border.Child = grid;

                // 将 Border 添加到 StackPanel 中
                page.Children.Add(border);
            }
        }

        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
    }
}
