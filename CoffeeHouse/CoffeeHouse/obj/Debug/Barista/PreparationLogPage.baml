   M S B A M L   `   `   `  ����  CoffeeHouse,    &CoffeeHouse.Barista.PreparationLogPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$-  'Журнал приготовлений��5       � 	Resources 5
         CoffeeButtonStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " ��6*   .�5      ��  � �� �	#FF5D40376+   �� 5       � ��
 �����6+   �� 5       � ��$ 8q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$	 100��6)   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��10q�6"   .+�5      �� $-��'Журнал приготовлений��6   $��18(�5      $
��Bold"�5      $��0,0,0,10q�5      ���	#FF5D40376*   �� 5        �� Orientation$ 
Horizontal=�6   $��0,0,0,15q�62   .+�5!      (  "System.Windows.Controls.DatePicker  -   5          Name$ StartDatePicker��5!      $��0,0,10,0q�5"      $	��120��5#      �� 5$      $
��по��6   $��Center=�5%      $��0,0,10,0q�5&        5'      -   5        $ EndDatePicker��5'      $��0,0,10,0q�5(      $	��120��5)      �� 5*      -   5        $��Фильтровать��5*      5,      5-      5.      &   System.Windows.Controls.DataGrid  50   
   -   5          Name$ PreparationLogGrid��50      $��1��51      	   AutoGenerateColumns	 .  
   CanUserAddRows
 .  $��10q�54         
IsReadOnly .    Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57      ,  &System.Windows.Controls.DataGridColumn   Header$ Дата��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 59   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 58   %   �� *Date+59   %     5:      $ Номер заказа��6%   $	 150 5<   %    5;   %   �� *OrderNumber+5<   %     5=      $ Напиток��6%   $	 250 5?   %    5>   %   �� *	DrinkName+5?   %     5@      $+ %Время приготовления��6%   $	 200 5B   %    5A   %   �� *PreparationTime+5B   %     5C      $ Статус��6%   $
 Auto 5E   %    5D   %   �� *Status+5E   %   5G      5H      5I      