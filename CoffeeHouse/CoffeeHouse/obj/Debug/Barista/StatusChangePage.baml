   M S B A M L   `   `   `  ����  CoffeeHouse*    $CoffeeHouse.Barista.StatusChangePage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$<  6Управление статусами заказов��5       � 	Resources 5
         CoffeeButtonStyle �� 
TargetType �� Property �� Value��%�  &	        '��     �� 5   
   " ��6*   .�5      ��  � �� �	#FF5D40376+   �� 5       � ��
 �����6+   �� 5       � ��$ 8q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$	 100��6)   �� 5       �@�Cursor$
 Hand��6'   5      �� 5   
   " ��6   .�5      ��  � ��$ Center=�69   �� 5       � ��$ Center=�6;   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5!   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   $��Stretch=�6G   .+�5"      �� $<��6Управление статусами заказов��6   $��18(�5#      $
��Bold"�5$      ���	#FF5D4037$��Center=�5&      �� 5(      -   5        $#��Обновить данные��5(      $��10,0,0,0q�5*      $��Right=�5,      5-      &   System.Windows.Controls.DataGrid  50   
   -   5          Name$ 
OrdersGrid��50      $��1��51         AutoGenerateColumns .  	   CanUserAddRows	 .  $��10q�54      
   SelectionMode$
 Single=�55         
IsReadOnly . ��57      �� *Orders+ �� SelectedItem 58      �� *SelectedOrder+   Columns 59      0  *System.Windows.Controls.DataGridTextColumn  5:      ,  &System.Windows.Controls.DataGridColumn   Header$ Номер заказа��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 5<   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5;   %   �� *OrderNumber+5<   %     5=      $ Тип заказа��6%   $
 1.5* 5?   %    5>   %   �� *	OrderType+5?   %     5@      $! Время создания��6%   $
 1.5* 5B   %    5A   %   �� *CreationTime+5B   %   4  .System.Windows.Controls.DataGridTemplateColumn  5C      $ Статус��6)   $
 1.5* 69      CellTemplate 5D      �� 5E      .R�5F      �� $��Center=�5H   '   ��5F   '   �� *AvailableStatuses+ 5G   '   �� *Status+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5H   '   5I      5K        5L      $ Действия��6)   $
 Auto 6;    5M      �� 5N      .R�5O      �� -   5        $#��Обновить статус��5O   %   5P   %   5Q      5S      5U      5V      5W      