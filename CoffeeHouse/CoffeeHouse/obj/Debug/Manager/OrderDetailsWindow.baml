   M S B A M L   `   `   `  ����  CoffeeHouse,    &CoffeeHouse.Manager.OrderDetailsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Детали заказа��5   	   $	��500��5   	   $	��800��6    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0 0 0 10q�6   ._�5      �� .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0 0 0 5q�66   .+�5      �� $��Номер заказа:��6    $
��Bold"�65   $	��120��6G   �� 5      ��6    �� *Номер_заказа+5      �� 5      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5      �� $��Статус:��6    $
��Bold"�6/   $	��120��6A   �� 5      -   5         ��Name$ StatusComboBox��5      $	��150��67    �� SelectedValue 6C   �� *Статус+.@�5      �� $��в ожидании��6'   �� 5      $��в процессе��6'   �� 5      $��завершен��6'   5      5      �� 5      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5       �� $ ��Дата создания:��6    $
��Bold"�66   $	��120��6H   �� 5!      ��6    �� *ВремяСоздания+ �� StringFormat$ {0:dd.MM.yyyy HH:mm}��5"      �� 5#      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5$      �� $��Сумма заказа:��6    $
��Bold"�65   $	��120��6G   �� 5%      ��6    �� *ИтоговаяСумма+$ {0:C}��5&      �� 5'      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5(      �� $ ��Способ оплаты:��6    $
��Bold"�66   $	��120��6H   �� 5)      ��6    �� *СпособОплаты+5*      �� 5+      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5,      �� $��Комментарий:��6    $
��Bold"�64   $	��120��6F   �� 5-      $	��300��6B   $��60��6N    �� TextWrapping$
 Wrap=�5.      	 �� AcceptsReturn	 . ��5-      �� *&$КомментарийКлиента+5.   2   5/      50      51      �� 54   
   $��1��6   ._�55      �� .+�56      �� $ ��Состав заказа:��6   $
��Bold"�62   $��0 0 0 5q�6D   &   System.Windows.Controls.DataGrid  57      -   5        
  Name$
 OrderItemsGrid��57         AutoGenerateColumns .     
IsReadOnly . $	��200��58   -      Columns 59      0  *System.Windows.Controls.DataGridTextColumn  5:      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Товар��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6Y   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *Название+6Y     5;      $ Количество��6-   $	 200 6`    6A   �� *Количество+6`     5<      $ Цена��6-   $	 100 6j    6;   �� *
Цена+$ {0:C}��6j     5=      $ 
Сумма��6-   $	 100 6l    6<   �� *
Сумма+$ {0:C}��6l   5?      5@      5A      �� 5D   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��0 10 0 0q�6W   .+�5E      �� -   5        $��Сохранить��5E      $	��100��6)   $��0 0 10 0q�65   6G   �� 5F      -   5        $��Закрыть��5F      $	��100��6'   63   5G      5H      5I      