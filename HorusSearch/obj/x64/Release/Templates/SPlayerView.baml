   M S B A M L   `   `   `  ����   HorusSearch!    SDKSample.MediaElementSound   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � 	Resources     SliderRepeatButton �� 
TargetType �� Property �� Value �� 
TargetType  SliderRepeatButton1 J� SnapsToDevicePixels  SliderThumb 
 Slider � RowDefinitions ]�Name ]� DecreaseRepeatButton	 ]� Thumb
 ]� IncreaseRepeatButton  Horizontal_Slider �� Triggers P� Property P� Value��%   &	        &	 �     &	 �    &	 -    7  7  7   &	 �    7  �� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� .  ��  � |� .  ��  � �� �� " ��.R��� $��3�����	#5DFFFFFF���	#5DFFFFFF$��0q��� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $��3�����#fff���#fff$��0q� . �� " n�.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� �� " n�.R�A� $��10��$��10�����#fff�� " ��.R��  �� $
��Auto ��� $
��Auto �#��z���� $
��Auto �.+�]� $ 
PART_Track��$��1�� �� $��Slider.DecreaseLarge��8��  	 n� 8�� 
 �� $��Slider.IncreaseLarge��8�� �� " ��.���  � |� .  ��  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle .  P�  �@��Orientation$ 
Horizontal=�. ���  � ��$ 21����  � ��$	 104����  � ��8   .���� -   $��genericViewer��$��0��$��1��$
��Auto��$��0,0,0,0q�
������ 
�������$��1,0,0,0q�$y�Visible=�._�� -   $��gridGeneric��$	��600��$
��10,4q� � ColumnDefinitions �� $��1* � �� $
��0.4* ��� $��0.05* ��� $��0.05* ��� $
��0.2* ��� $
��0.4* �.+��� -   $��IcoApp��$��0��$��0��$	��150��$	��150��$��25,25,25,25q��� -   $��NameApp��$��1��$��0��$��10,2,3,2q�$��Center=�$��Application��$��20(� " SystemBaseMediumHighColorBrush#���  �� -   $��
VersionApp��$��2��$��0��$��10,2,3,2q�$��Center=�$��Application��$��15(����	#D3808080�� -   $��playerStack��$��3��$��0��$
��Auto��$��0,0,0,0q����	#5A000000$��0q� �� CornerRadius$ 10��._��� .+�r� -   $��McMediaElement��$	��550��$
��Auto�� r� LoadedBehavior$ Manual=� r� ScrubbingEnabled .  r� UnloadedBehavior$
 Stop=���  �� Orientation$ Vertical=�.+��� $	��151��$��51��$��	0,-90,0,0q�$��Center=�$��Center=�
������  J� Opacity$ .5Q�$ 
Horizontal=�.+��� -   $��
playButton��$��40��$��40��$��60,5,5,5q�$'��!..\Resources/images/UI_play_w.png���� $	��446��$��37��$��	0,-40,0,0q�$��Center=�$��Center=�
������ $ 
Horizontal=�.+��� -	    ��Name$ timelineSlider��$	��300��$��0,0,0,0q�$��Center=� �� IsMoveToPointEnabled .  �� IsSnapToTickEnabled . #��[ �� -
   $��40��$��40��$��0,-2,0,0q�$)��#..\Resources/images/UI_volume_w.png���� -   $ volumeSlider��$��40��$	��0,8q�$��Center=� � Maximum$ 1Q� � Minimum$ 0Q�#��[  � Value$	 0.5Q�� $��4��$��0��$
��10,4q� �� $	��.5* ��� $	��.5* � �� $��30 ��� $��20 ��� $��20 ��� $��20 ��� $��20 ��� $��20 �.+��� $��1��$��0��$��3,2,3,2q�$��Right=�$
��Kind��$
��Bold"����	#D3808080�� -   $
��Kind��$��1��$��1��$��10,2,3,2q�$��Application��#���  �� $��2��$��0��$��3,2,3,2q�$��Right=�$
��Size��$
��Bold"����	#D3808080�� -   $
��Size��$��2��$��1��$��10,2,3,2q�$��0000 MB��#���  �� $��3��$��0��$��3,2,3,2q�$��Right=�$��Created��$
��Bold"����	#D3808080�� -   $��Created��$��3��$��1��$��10,2,3,2q�$��
11/22/2222��#���  �� $��4��$��0��$��3,2,3,2q�$��Right=�$��Modified��$
��Bold"����	#D3808080�� -   $��Modified��$��4��$��1��$��10,2,3,2q�$��
22/11/1111��#���  �� $��5��$��0��$��3,2,3,2q�$��Right=�$��Last opened��$
��Bold"����	#D3808080�� -   $��
LastOpened��$��5��$��1��$��10,2,3,2q�$��
22/11/1111��#���  