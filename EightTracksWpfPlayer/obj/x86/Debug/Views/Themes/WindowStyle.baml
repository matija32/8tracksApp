   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   �    EightTracksWpfPlayer?   9EightTracksWpfPlayer.Views.Themes.WindowCustomBorderStyleR NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries   WindowCorderRadius  TitleCorderRadius  TitleBarRadialButton �� 
TargetType �� Property �� Value  CloseRadialButton �� BasedOn �� 
TargetType �Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth	   Color
   
BlurRadius ��Name �� Triggers P� Property P� Value �� 
TargetName :� RoutedEvent �� 
TargetName �� TargetProperty	 W� To g� Duration  MinimizeRadialButton  MaximizeRadialButton  WindowBorder �� Triggers �� Binding �� RelativeSource �� Path �� Value  TitleBarBackgroundBorder  TitleBarBackgroundBrush 	 
WindowRoot 
 WindowRootMargin  RectBorderStyle  WindowTemplate � 	Resources  Key ��Name �� Orientation � Icon �� CenterX �� CenterY  �� ScaleX! �� ScaleY  	TextBrush" � Title# �� SelectedIndex$ �� Mode% �� TargetNullValue& �� FallbackValue
' � Tag( ��Name  SkinWindowStyle   6   D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     6W   .sys&clr-namespace:System;assembly=mscorlib  6�     5      %h  &	        &	 !     &	 B     &	 I    7  &	 �    7  &	 �    7  &	 i	    7   &	 &    7  &		 d    &	     &	 �    7  7  7  7  7  7  7  7  7  7  7  &	 �    7  ��5      	10,10,5,5��66   ��5      	10,10,0,0��65   �� 5
      " ��6&   .�5      ��  �@J�SnapsToDevicePixels . 6*   �� 5       �@�OverridesDefaultStyle . 6,   �� 5       � ��$ 13��6    �� 5       � ��$ 13��6   �� 5       �@J�Opacity$	 0.9Q�6   5      �� 5      " ��6#   8   .�5      ��  � �� 5      �� 5      " ��6   .R�5      � $ 
ButtonRoot��6    5        5   	   $ 0Q�6   $		 Red��6*   $
 0Q�66   .+�5      �� $ Image��6   $��Images\normal close.png��6   $��Center=�6>   $��Center=�6[   $��14��6v   $��14��6�   5   	   5       5      P� 5        �@J�IsMouseOver . . �5!   	   �� $ Image��6    � ��$" Images\highlighted close.png��65   5"   	   P� 5#       � �� . 5$   	   :� 5&      $ 
MouseEnter��6   .[�5'   	   �� .��5(   
   �� .��5)      W� $ 
ButtonRoot��6   $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)�6>   $ 15X�6�   $ 0:0:0.3D�6�   W� 5+      $ Opacity�6   $ 1X�6?   $ 0:0:0.5D�6F   5,      5-   
   5.   	   :� 50      $ 
MouseLeave��6   .[�51   	   �� .��52   
   �� .��53      W� $ 
ButtonRoot��6   $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)�6>   $ 0X�6�   $ 0:0:0.2D�6�   W� 55      $ Opacity�6   $	 0.9X�6?   $ 0:0:0.5D�6H   56      57   
   58   	   5:      5<      5=      �� 5@      " ��6&   8   .�5A      ��  � �� 5B      �� 5C      " ��6   .R�5D      � .+�5E      �� $ Image��6   $ ��Images\normal minimize.bmp��6   $��Center=�6A   $��Center=�6^   $��14��6y   $��14��6�   5F       5G      P� 5H       �@J�IsMouseOver . . �5I   	   �� $ Image��6    � ��$% Images\highlighted minimize.bmp��65   5J   	   P� 5K       � �� . 5L   	   5N      5P      5Q      �� 5T      " ��6&   8   .�5U      ��  � �� 5V      �� 5W      " ��6   .R�5X      � .+�5Y      �� $ Image��6   $ ��Images\normal maximize.bmp��6   $��Center=�6A   $��Center=�6^   $��14��6y   $��14��6�   5Z       5[      P� 5\       �@J�IsMouseOver . . �5]   	   �� $ Image��6    � ��$% Images\highlighted maximize.bmp��65   5^   	   P� 5_       � �� . 5`   	   5b      5d      5e      �� 5h      " ��6   .�5i      ��  �@��CornerRadius8   6#   �� 5j       � �� �#ABABAB6!   �� 5k       � ��
 �iii�6"   �� 5l       � ��$ 1q�6&   �� 5m       � ��$ Stretch=�6*   �� 5n       � ��$ Stretch=�6(    5p      �� 5q       6   ��  �� *TemplatedParent+$ WindowState� 	Maximized6g   .a�5r      ��  �@��CornerRadius$ 0��6%   5s      5v      �� 5x      " ��6*   .�5y      ��  � ��$ 24��6   �� 5z       �@��CornerRadius8   6#   �� 5{       � ��# �  6!    5}      �� 5~       6   ��  �� *TemplatedParent+$ WindowState� 	Maximized6g   .a�5      ��  �@��CornerRadius$ 0��6%   5�      5�      �� 5�      " �6    5�      �� 5�       6   ��  �� *TemplatedParent+$ WindowState� 	Maximized6g   .a�5�      ��  � ��# � 
 6   5�      5�      �� 5�      " ��6!   .�5�      ��  � |� .  6    �� 5�       � ��
 ���� 6   5�      �� 5�      " �6*    5�      ���5�      BorderWidth 7 �6'   5�      .R�5�      � $ 
WindowRoot��6	   -   5        #��� 	 5�      .+�5�      �� $ WindowFrame��6   #���  6!   ._�5�      � $ WindowLayout��6   .+�5�      � $��0q�6   $	��Top=�6   .+�5�      �� -   5        #���  5�      6c   �� 5�      $ 
Horizontal=�6   .+�5�   	   �� $ IconApp��6   #��z 6    $
��Left=�6@   $	��Top=�6[   z�5�   
   �� 5�      $ 0Q�6   $ 6Q�6&   $	  0.7Q�62   $	! 0.7Q�6?   5�   
   �� 5�   	   -   5        #���  5�      #��z" 6<   $��Center=�6[   $��Center=�6x   $��SemiBold"�6�   6�   5�   	   �� 5�      $	��0,0q�6   $ 
Horizontal=�6    $��Right=�69   $��Center=�6U   .+�5�   	   �� -   5        $��0,0,5,0q�5�      $��16��6#   $��8(�6/   $��3q�6<   $y�Hidden=�6�   # 6s   �� $ Settings[SelectedTheme]�$$ TwoWay=�$% 3��$& 3��.@�5�   
   �� $
' None��6   .��6"   None6(   �� 5�   
   $' BureauBlack��6   .��6)   Bureau Black67   �� 5�   
   $' 
BureauBlue��6   .��6(   Bureau Blue65   �� 5�   
   $' ExpressionDark��6   .��6,   Expression Dark6=   �� 5�   
   $' ExpressionLight��6   .��6-   Expression Light6?   �� 5�   
   $' 	ShinyBlue��6   .��6'   
Shiny Blue63   �� 5�   
   $' ShinyRed��6   .��6&   	Skiny Red61   �� 5�   
   $' WhistlerBlue��6   .��6*   Whistler Blue69   5�   
   �� 5�   	   -   5        8��  $��Right=�5�   U   $��Center=�6q   $��0,0,5,0q�6�   �� 5�   	   -   5        8�� $��Right=�5�   U   $��Center=�6q   $��0,0,5,0q�6�   �� 5�   	   -   5        8�� $��Right=�5�   O   $��Center=�6k   $��0,0,10,0q�6�   5�   	   5�      �� 5�      $ WindowContent��6   $��0,24,0,0q�6%   ._�5�      �� .u�5�   	   �� 5�   	   5�      �� 5�      $( leftSizeGrip��6   8�� $��7��6P   $
��Left=�6Z   �� 5�      $( rightSizeGrip��6   8�� $��7��6Q   $��Right=�6[   �� 5�      $( topSizeGrip��6   8�� $��7��6O   $	��Top=�6Z   �� 5�      $( bottomSizeGrip��6   8�� $��7��6R   $��Bottom=�6]   �� 5�      $��topLeftSizeGrip��6   8�� $��7��6Q   $��7��6[   $
��Left=�6f   $	��Top=�6�   �� 5�      $��bottomRightSizeGrip��6   8�� $��7��6U   $��7��6_   $��Right=�6j   $��Bottom=�6�   �� 5�      $��topRightSizeGrip��6   8��	 $��7��6R   $��7��6\   $��Right=�6g   $	��Top=�6�   �� 5�      $��bottomLeftSizeGrip��6   8��
 $��7��6T   $��7��6^   $
��Left=�6i   $��Bottom=�6�   5�      5�      5�      5�      �� 5�      " �6!   .�5�      ��  �@�WindowStyle$
 None=�6"   �� 5�       �@�AllowsTransparency . 6)   �� 5�       �@�
ResizeMode$ CanResizeWithGrip=�6!   �� 5�       � ��
 ���� 6!   �� 5�       � ��$	 150��6    �� 5�       � ��$	 250��6   �� 5�       �@J�Opacity$	 1.0Q�6   �� 5�       � ��8   6    5�      �� 5�       6   �� *WindowState+ 	Maximized61   6E   5�      5�      