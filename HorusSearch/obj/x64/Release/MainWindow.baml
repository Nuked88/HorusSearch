   M S B A M L   `   `   `  ���� W  SCefSharp.Core, Version=79.1.360.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138:-clr-namespace:CefSharp;assembly=CefSharp.CoreCefSharp  D @FluentWPF, Version=0.8.0.6, Culture=neutral, PublicKeyToken=nullP6clr-namespace:SourceChord.FluentWPF;assembly=FluentWPFSourceChord.FluentWPF F BHorusSearch, Version=0.8.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  YascK1clr-namespace:HorusSearch.Templates;assembly=YascHorusSearch.Templates V RCefSharp.Wpf, Version=79.1.360.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf    WpfApp1.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     =cef-clr-namespace:CefSharp;assembly=CefSharp.Core     @d2http://schemas.microsoft.com/expression/blend/2008     Efw6clr-namespace:SourceChord.FluentWPF;assembly=FluentWPF     'localclr-namespace:WpfApp1     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     G	templates1clr-namespace:HorusSearch.Templates;assembly=Yasc     @wpf0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     -   $��mainw��  � Title$
  Yasc��$	��866��$	��442��)  #SourceChord.FluentWPF.AcrylicWindow   Enabled .    AcrylicWindowStyle$
 None=�$��0q� � ShowInTaskbar .  
 � Top$ 0�� � UseLayoutRounding .  � WindowStartupLocation$ Manual=� � 	Resources    resultsList �� 
TargetType	 �� Property
 �� Value  ListTextSelectedColor  ListSelectedColor  _ListBoxItemStyle �� 
TargetType �� Padding J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName  SystemBaseLowColorBrush " SystemBaseMediumHighColorBrush  colorbar  SystemBaseMediumHighColor �� Color  StandardBorderBrush 	 StandardBackgroundBrush 
 HoverBorderBrush  SelectedBackgroundBrush  SelectedForegroundBrush  DisabledForegroundBrush  NormalBrush  NormalBorderBrush  HorizontalNormalBrush  HorizontalNormalBorderBrush  ListBoxBackgroundBrush �� 
StartPoint �� EndPoint � Offset � Color  StandardBrush  
GlyphBrush  PressedBrush  ScrollBarLineButton �� CornerRadius �� Path �� RelativeSource  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBar � ColumnDefinitions � RowDefinitions ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton  HorizontalScrollBar �� Triggers  FavsScrollViewer  �� ScrollableHeight! � Maximum" �� ViewportHeight# �� ViewportSize*$ �� #ComputedVerticalScrollBarVisibility% �� VerticalOffset& � Value' �� ScrollableWidth( �� Orientation) �� ViewportWidth,* �� %ComputedHorizontalScrollBarVisibility+ �� HorizontalOffset  customTemplate, � 	Resources  Key- �� RadiusX. �� RadiusY  DisabledForegroundBrushtb  DisabledBackgroundBrushtb   RichTextStyle/ �� BasedOn0 ��Name1 ��Name!2 �� IsDeferredScrollingEnabled��%�  &	        &	 �    &	 �    &	 �     &	 p    &	 |    &		 �    &	
 �    &	 �    &	 �    &	 �    &	      &	     &	 .    &	 A    &	 Q    &	 �    &	 >    &	 U    &	 �    7  7  7  7  7  &	 b    &	     7  &	 �    7  7  7  7  7  7  7  &	 �    7  7  7  7  7  7  7  '�� �
    7  7  &	 �    &	     &	 b    &	 n    '�� �    7  7  &	  D    �� " ��.��� 	 � ��$
 Hidden=��� 	 � ��$

 Auto=��� 	 � ��
 . �� 	 �@��PanningMode$

 Both=��� 	 �@��IsFlicksEnabled
 .  �� 	 � ��$
 Center=��� �� �� " ��.��� 	 � ��
 �� " ��.R��� $��_Border��$ 2q� . ._���  P�  �@��
IsSelected . . ��� $ _Border��#
 �  	 � ���� #
 �  	 � ���� # �  �� $ Transparent���� $ Transparent���� $ Transparent���� $ Transparent���� $ White���� $ Transparent���� $ Transparent���� $ Transparent���� $ #FFFFFF���� $
 #fff���� $	 0,0&�$ 1,0.001&���� � $	 0.0Q�$ Transparent��� $	 0.6Q�$ Transparent��� $	 1.2Q�$ Transparent���� $	 0,0&�$	 0,1&���� � $	 0.0Q�$ Transparent��� $	 1.0Q�$ Transparent���� $ Transparent���� $	 0,0&�$	 0,1&���� � $	 0.0Q�$
 #BBB��� $	 0.1Q�$
 #000��� $	 0.9Q�$
 #000��� $	 1.0Q�$
 #000���� " ��.��� 	 �@J�SnapsToDevicePixels
 . �� 	 �@�OverridesDefaultStyle
 . �� 	 � |�
 .  �� 	 � ��
 �� " ��.R��� $��Border��$��1q�8��  8�� $��1q�$ 2��._�F� $��Center=�$��Center=�8�� ���� $ Content� �� *TemplatedParent+ P�  � �� . . ��� $ Border��8
  	 � ��P�  � {� .  . ��� 8
  	 � ���� " ��.��� 	 �@J�SnapsToDevicePixels
 . �� 	 �@�OverridesDefaultStyle
 . �� 	 � ��
 .  �� 	 � |�
 .  �� 	 � ��
 �� " ��.R��� 
������ �� " n�.��� 	 �@J�SnapsToDevicePixels
 . �� 	 �@�OverridesDefaultStyle
 . �� 	 � ��
 .  �� 	 � |�
 .  �� 	 � ��
 �� " n�.R��� $��5��8��  #��z��$��0q�$ 2���� " ��.R��  �� $��0 ��� $��20 � �� $��18���� $��0.00001* ��� $��18��.+��� $��3�����#F0F0F0$ 2���� $��0��$��1��$��18��$��ScrollBar.LineUpCommand��$��M 0 4 L 8 4 L 4 0 Z��8��  ]� $��
PART_Track��$��1��$��1����.  �� $��ScrollBar.PageUpCommand��8��  n� $��1,0,1,0q�8�� 8�� 8��  �� $��ScrollBar.PageDownCommand��8�� �� $��3��$��1��$��18��$��ScrollBar.LineDownCommand��$��M 0 0 L 4 4 L 8 0 Z��8�� �� " ��.R��  �� $��18���� $��0.00001* ��� $��18��.+��� $��3�����#F0F0F0$ 2���� $��0��$��18��$��ScrollBar.LineLeftCommand��$��M 4 0 L 4 8 L 0 4 Z��8��  ]� $��
PART_Track��$��1��$��2����.   �� $��ScrollBar.PageLeftCommand��8��  n� $��0,1,0,1q�8�� 8�� 8��  �� $ ��ScrollBar.PageRightCommand��8�� �� $��3��$��18��$ ��ScrollBar.LineRightCommand��$��M 0 0 L 4 4 L 0 8 Z��8�� �� " ��.��� 	 �@J�SnapsToDevicePixels
 . �� 	 �@�OverridesDefaultStyle
 .  P�  �@��Orientation$ 
Horizontal=�. ��� 	 � ��$

 Auto���� 	 � ��$
 18���� 	 � ��8
   P�  �@��Orientation$ Vertical=�. ��� 	 � ��$
 18���� 	 � ��$

 Auto���� 	 � ��8
  �� " ��.��� 	 �@�OverridesDefaultStyle
 . �� 	 � ��
 �� " ��.R��  �� $
��Auto ���  �� �� $
��Auto �.+��� $��1���� $��PART_VerticalScrollBar��#! z  ## z" #y�z$ #& z% �� $��PART_HorizontalScrollBar��$��1��$��1��#! z' $( 
Horizontal=�## z) #y�z* #& z+ �� " �, ���� progresscolor $	 0 0&�$	 1 0&�.��� $ 0Q�$ Orange��� $	 0.5Q�$ White��� $ 1Q�$ Green��.R�� .+��� $��
PART_Track��
������ $- 5��$. 5��{� $��PART_Indicator��#��z��$
��Left=�._�� $��	Animation��}�. .+��� $��PART_GlowRect��$	��100��$
��Left=�#���  �� # �  �� $ 	#02FFFFFF���� " ��.��� 	 � ��
 �� " ��.R��� $��Bd��#��z��#��z��#��z�� . ._��� $��PART_ContentHost��#��z��# z  P�  � {� .  . ��� 8
   	 � ���� 8
  	 � ���� " ��/ Z� .��� 	 � ��$
 3q��� 	 � ��$
 0,5,0,0q��� 	 � ��
 �� " ��.R��� $0 bg��#��z�����#000$��0q�._��� $1 PART_ContentHost����.  2 .  P�  � {� .  . ��� $ bg��	 � ��

 ���� .��� $��0,0,0,0q�  ! SystemChromeAltLowColorBrush#��� !  �� $��1* � �� $��58 ��� $��* �.+��� -   3 ��Name$3 lbSuggestion��$��0��$��0��$	��595��$��0,12,0,0q�$��	10,0,10,7q�$
��Left=����	#02FFFFFF$��0q�$��20(�#���  4 J� Opacity$	4 0.5Q�$y�Visible=��� -   $��search��$��0��$��0��$	��595��$��0,0,0,0q�$��	10,0,10,7q�$
��Left=����	#02FFFFFF$��0q�5 �� 
CaretBrush#5 �  $��20(�#���  6 �� MaxLines$6 1��7 �� TextWrapping$7 NoWrap=�.��.��� $��2��$��1��$��0,0,0,0q� �� -   8 �Name$8 reswCol��$
��0.4* ��� -   $8 prewCol��$
��0.6* � �� $��1* �.+��� -   $��resultsList��$��0��$��0��$
��Auto��$��0,10,0,0q�$
��Left=�$	��Top=�$��Stretch=�
������ $��0q�$��18(�#���  #���  #���   ���� .R�� $
��10,4q� �� $��1* � �� $
��0.8* ��� $
��0.1* �.+��� 9 �� Orientation$9 Vertical=�.+�� $	��0,0q� �� $	��919 � �� $
��0.8* �.+��� $��0��$��0��: �� Padding$: 10,3,0,0q�$��Stretch=����	#18FFFFFF$��14(�$��Medium"�#���  ���� *hLabel+���� *mLabel+���� *	textLabel+y��� *vLabel+�� $9 
Horizontal=�.+��� $��iconapp��$��1��$��0��$��20��$��0,0,20,0q����� *	appicon+�� $
��rbox��$��1��$��0��$��23��$��-9,0,0,0q�$��Stretch=�$��Center=�
������ $��0q�#���  {�.  $	4 0.8Q�#��[  ���� *Sizeb+  WpfApp1.RichTextBoxHelper;   DocumentXaml; �� *TitleFormatted+ � -   $��browseContent��$��0��$��1��$
��Auto��$��0,0,0,0q�
������ 
�������$��0q�<  � NavigationUIVisibility$< Hidden=�=  � Source$ = Templates\BrowserView.xaml?�� -   $
��pBar��$	��838��$��58��$
��Left=�$	��Top=����	#01910E0E���	#FFAA3030���	#B20078D7> � IsIndeterminate> . $	4 0.5Q�#��[ $& 30Q�