   M S B A M L   `   `   `  ����  CoffeeHouse*    $CoffeeHouse.Admin.MenuManagementPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Aav2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        N� Title$  MenuManagementPage��5      .��5	      � $��10q�6   .+�5
   
   �� $��	0,0,-14,0q�6   .+�5      �� $#��Управление меню��6   $��16(�6/   $��0 0 0 10q�6=   �� 5      ._�5      �� .+�5      ��  �� Orientation$ 
Horizontal=�6!   $��0 0 0 10q�6:   .+�5      �� -   5         ��Name$ AddItemButton��5   !   $��Добавить��68   $
��10 5q�6K   6Z   �� 5      -   5        $ EditItemButton��5   !   $ ��Редактировать��69   $
��10 5q�6Q   $��10 0 0 0q�6`   6r   �� 5      -   5        $ DeleteItemButton��5   !   $��Удалить��6;   $
��10 5q�6M   $��10 0 0 0q�6\   6n   5      &   System.Windows.Controls.DataGrid  5      -   5          Name$ MenuItemsGrid��5         AutoGenerateColumns .     CanUserAddRows .     SelectionMode$ Single=�5         SelectionUnit$ FullRow=�66   $��0 10 0 0q�6N      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��61   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 50 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *IDТовара+6Z     5      $	 Название��61   $	
 150 6`    6C   �� *Название+6`     5      $	 Категория��61   $	
 120 6b    6D   �� *Категория+6b     5      $	 Цена��61   $
 80 6X    6?   �� *
Цена+6X   4  .System.Windows.Controls.DataGridCheckBoxColumn  5      $	 Доступность��65   $	
 110 6j    6J   �� *Доступность+6j     5      $+	 %Время приготовления��61   $	
 170 6u    6N   �� *&$ВремяПриготовления+6u   5      5       5!      5"      5#      5$      