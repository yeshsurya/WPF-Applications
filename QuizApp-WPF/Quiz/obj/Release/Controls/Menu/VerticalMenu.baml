   M S B A M L   `   `   `  ���� ?  ;Quiz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:QuizQuiz  D @Quiz.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Quiz.Core;assembly=Quiz.Core	Quiz.Core     Quiz.VerticalMenu   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $localclr-namespace:Quiz     ;core*clr-namespace:Quiz.Core;assembly=Quiz.Core     .���� 
����������� *Items+���� .R�� .+���    ForegroundLightBrush#��[    �� CornerRadius$  8�� � Triggers :�  :� RoutedEvent$ 
MouseEnter��.[��� .���� .����   WordBlue	 �� To# [  g� Duration$ 0:0:0D� �� TargetProperty$ Background.Color���   ForegroundLight# [ $ 0:0:0D�$ Foreground.Color� �� 
TargetName$
 icon���� # [ $ 0:0:0D�$ Foreground.Color�$
 text��:� $ 
MouseLeave��.[��� .���� .���� # [ $ 0:0:0D�$ Background.Color���   ForegroundDark# [ $ 0:0:0D�$ Foreground.Color�$
 icon����   ForegroundVeryDark# [ $ 0:0:0D�$ Foreground.Color�$
 text�� J� InputBindings d�  d� MouseAction$ 	LeftClicke�	 �� Command	 �� *ChangePageCommand+._�� $��8q�
 � ColumnDefinitions
 �� $��25 ��� $��* �.+��� $
��icon�� �� Padding$ 5q�$��Center=�$��Center=�  FontAwesome#��[   ForegroundDarkBrush#��[ ���� *Icon+ �� 	Converter )   #Quiz.IconTypeToFontAwesomeConverter  �� $
��text��$��1��$��5 0 5 0q�  ForegroundVeryDarkBrush#��[   LatoRegular#��[ $��Center=����� *Text+