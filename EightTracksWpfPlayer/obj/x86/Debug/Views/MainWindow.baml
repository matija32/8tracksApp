   M S B A M L   `   `   `  ����S  OEightTracksPlayer, Version=1.2.5135.38043, Culture=neutral, PublicKeyToken=nullfEclr-namespace:EightTracksPlayer.ViewModels;assembly=EightTracksPlayerEightTracksPlayer.ViewModels  5   	   zOclr-namespace:EightTracksPlayer.Utils.ViewExtensions;assembly=EightTracksPlayer&EightTracksPlayer.Utils.ViewExtensions  5   	    EightTracksWpfPlayer+   %EightTracksWpfPlayer.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   V
ViewModelsEclr-namespace:EightTracksPlayer.ViewModels;assembly=EightTracksPlayer  5   	   dViewExtensionsOclr-namespace:EightTracksPlayer.Utils.ViewExtensions;assembly=EightTracksPlayer  5   	   -   5          �Name$  Window��5   	      SkinWindowStyle#���   5   	    � UseLayoutRounding .  � Icon$" ..\Resources\Icon8tracks.ico��5	   	    � Title$ 8tracks player 1.4��5
   	    � SizeToContent$ Manual=�5   	    � DataContext 5   	   ��  �� RelativeSource �� *Self+��5   	   �� *Settings[Height]+ �� Mode$ TwoWay=�  SettingConv �� 	Converter# [ ��5   	   �� *Settings[Width]+# [ 	 � Left	 5   	   �� *Settings[Left]+$ TwoWay=�# [ 

 � Top
 5   	   �� *Settings[Top]+$ TwoWay=�# [  � 	Resources 5      <   6EightTracksPlayer.Utils.ViewExtensions.WatermarkHelper  WatermarkHelper  EntryFieldStyle �� 
TargetType �� Property �� Value  NoChromeButton �� 
TargetType �Name J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName  MixItemThumbTemplate �� Orientation �� CornerRadius �� Path �� TextAlignment  HoveredMixItemButtonStyle �� BasedOn �� Triggers  LikeMixImageStyle �� Binding �� Value  HoveredMixItemThumbTemplate � ToolTip 	 ListBoxBackgroundStyle �� 	Resources  �� Color 
 MixItemThumbContainerStyle  MixItemDetailsTemplate! � Cursor   MixItemDetailsContainerStyle  MixListBoxItemsStyle1   +EightTracksPlayer.ViewModels.MixDisplayMode"   Thumbs  UserNotLoggedInDataTemplate  WindowBackgroundBrush;   5EightTracksPlayer.Utils.ViewExtensions.FocusExtension#   	IsFocused$ �� UpdateSourceTrigger% G�NameA   ;EightTracksPlayer.Utils.ViewExtensions.PasswordBoxAssistant&   BindPassword'   BoundPassword( J� InputBindings
) �� Key* �� Command  UserLoggedInDataTemplate  NoFocusVisualStyle+ �� StrokeDashArray  LoginFormStyle  DownloadTrackButtonStyle, �� ElementName  TrackItemColoring  LastSongColoring  TrackItemTemplate  TrackItemContainerStyle  TrackListBoxItemsStyle��%�(  &	       &	      &	 �     &	 <    &	 �    7  &	 .    &	     7  7  7  &		 o    &	
 _    7 	 7  7  &	 ,    7  7  7  &	 |    7 	 7  &	 �    7  7 
 &	     &	 f    &	 �    &	 �    79�7  7  &	 �     7��&	 �"    &	 �#    7��&	 �$    7  7  7  7  7  &	 �&    7 	 7  &	 ;'    7    5%      6#   �� 5'      " �6"   .�5(      ��  � ��$ Stretch=�6+   �� 5)       � ��$ Center=�6)   5*      �� 5,      " ��6!   .�5-      ��  � ��
 ���� 6"   �� 5.       � ��$ 1q�6'   �� 5/       � ��# � ��6"   �� 50       � ��$ Center=�62   �� 51       � ��$ Center=�60   �� 52       � ��$ 1q�6   �� 53       � �� 54      �� 55      " ��6   .R�56      � $ Chrome��6   #��z��6    . .+�57   	   �� #��z��6   #��z��6]   ��. # z 6�   #��z��6�   58   	    59      P� 5:   	    � {� .  . �5<   
   �� $ Chrome��6$    �@J�Opacity$	 0.5Q�68   5=   
   P� 5>   	    � �� . . �5?   
   �� $ Chrome��6$    �@J�Opacity$
 0.75Q�68   5@   
   5B      5D      5E      �� 5H      .R�5I      �� $ Vertical=�6   $	��Top=�6'   $	��120��6?   $	��170��6K   .+�5J      �� 
�������$��2q�6    $ 4��64   $��5q�6E   ._�5K      �� $
��Fill=�6.   $	��120��6=   $	��Top=�6J   ��6   �� $ CoverUri�6J   5L      �� 5M      $��40��6+   $ Center=�67   $
��Wrap=�6N   $��CharacterEllipsis=�6b   
���������6   �� $
 Name�6�   5N      5O      �� 5R      " ��6
   8   .�5S      ��  � ��$ 50��6   �� 5T       � ��$ 50��6    5U      P� 5V       �@J�IsMouseOver .  . �5W      ��  �@J�Opacity$	 0.3Q�6!   �� 5X       � ��
 ���� 6$   �� 5Y       � ��$ 0q�6)   �� 5Z       � ��
 ���� 6%   5[      5]      �� 5_      " ��6
   .�5`      ��  � ��$ ../Resources/NotLike.png��6    5a      �� 5b       6   �� $ LikedByCurrentUser� True6>   .a�5c      ��  � ��$ ../Resources/Like.png��6    5d      5f      �� 5h      .R�5i      �� .+�5j      �� ._�5k      ]� $	��120��6   $	��130��6   .+�5m      �� $	��Top=�6   $ 
Horizontal=�6(   $��5,0,0,0q�6A   .+�5n   	   �� $!��../Resources/PlaysCount.png��6   $��16��64   $��16��6@   $��0,0,4,0q�6K   �� 5o   	   $��0,0,4,0q�6.   ��6   �� *
PlaysCount+6.   �� 5p   	   $!��../Resources/LikesCount.png��6   $��16��64   $��16��6@   $��0,0,4,0q�6K   �� 5q   	   $��0,0,4,0q�6.   ��6   �� *
LikesCount+6.   5r   	   �� 5t      $��5,0,5,5q�6   $
 Left=�6>   $
��Wrap=�6S   $	��Top=�6g   $��CharacterEllipsis=�6|   
�������$��55��6�   ��6#   �� $
 Name� 6�   �� $
 Name��� 5w      $ 
Horizontal=�6   $��Bottom=�6,   .+�5x   	   �� $��5,0,5,5q�6   $��Bottom=�6!   8��  ��6o   �� *Play+.��5y   
   �� $��../Resources/Play64.png��6   5z   
   �� 5{   	   $��5,0,5,5q�6   $��Bottom=�6!   8�� ��6o   �� *
ToggleLike+.��5|   
   �� 8�� 6   5}   
   5~   	   5      5�      ]� 5�      $	��120��6   $��40��6   5�      5�      �� 5�      " ��6
    5�      ���� 5�      $  	LimeGreen��6H   (��       6   * SystemColors.HighlightBrushKey+)6H   �� 5�      $  	DarkGreen��6F   (��       6   *SystemColors.ControlBrushKey+)6F   5�      5�      �� 5�      " ��6
   8   .�5�      ��  � ��8  6'   �� 5�       � |� .  6!    5�      P� 5�       �@J�IsMouseOver . . �5�      ��  � ��8  6)   5�      5�      �� 5�      .R�5�      ]� $	��130��6   $��0,0,0,10q�6   .+�5�      �� $ Vertical=�6   $	��Top=�6(   $	��120��6@   $	��120��6L   .+�5�      �� $
��Left=�6   
�������$��2q�67   $ 4��6K   $��5q�6\   ._�5�      �� $
��Fill=�6/   $	��120��6>   $	��Top=�6K   ��6   �� $ CoverUri�6K   5�      5�      �� 5�      $��60��6   $��Right=�6   $��5,5,5,5q�66   $ Vertical=�6G   $��Right=�6^   .+�5�      �� $��5,5,5,5q�6   8��  $��Right=�6R   ��6n   �� *Play+.��5�      �� $��../Resources/Play64.png��6   5�      �� 5�      $��Right=�6   $��5,5,5,5q�6*   8�� ��6n   �� *
ToggleLike+.��5�      �� 8�� 6   5�      5�      �� 5�      $ Vertical=�6   $��Bottom=�6(   $��5,5,5,5q�6@   .+�5�      �� $ 
Horizontal=�6   .+�5�      �� $!��../Resources/PlaysCount.png��6   $��16��63   $��16��6?   $��0,0,5,0q�6J   �� 5�      $��0,0,10,0q�62   ��6   �� $ 
PlaysCount�62   �� 5�      $!��../Resources/LikesCount.png��6   $��16��63   $��16��6?   $��0,0,5,0q�6J   �� 5�      $��0,0,10,0q�62   ��6   �� $ 
LikesCount�62   �� 5�      $#��../Resources/DurationInfo.gif��6    $��16��6G   $��16��6S   $��0,0,5,0q�6^   �� 5�      $��0,0,0,0q�6M   ��6    �� $ TotalNumberOfTracks�6M   �� 5�      $��	 tracks (��6    �� 5�      ��6    �� $ TotalDurationAsString��� 5�      $��)��6    5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� $��Tags: ��6   �� 5�      ��6   �� $ TagList� 6/   �� $ TagList�5�      5�      �� 5�      $��5,5,5,5q�6   $ Vertical=�6"   .+�5�      �� $
! Hand��6   �� 5�      -   5        
������ ��5�      �� $ 
RestfulUri�.�6|   .�5�   	   �� $��CharacterEllipsis=�6.   
�������$
��Bold"�6b   ��6   �� $
 Name� 6t   �� $ 
RestfulUri�5�   	   5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� $��Published by ��6   �� 5�      
���������6   �� $ Author�6.   �� 5�      $
�� on ��6   �� 5�      ��6   �� $ FirstPublishedAt�5�      �� 5�      $��50��6   $��CharacterEllipsis=�6B   $
��Wrap=�6c   $��50��6w   
�������$��Italic$�6�   ��6    �� $ Description� 6�   �� $ Description�5�      5�      5�      �� 5�      " ��6
   8   .�5�      ��  � ��8  6'   �� 5�       � ��$ Stretch=�62   �� 5�       � |� .  6!    5�      ���� 5�      $  Green��6H   (��       6   * SystemColors.HighlightBrushKey+)6H   �� 5�      $  	LightGray��6F   (��       6   *SystemColors.ControlBrushKey+)6F   5�      5�      �� 5�      " ��6
   .�5�      ��  � ��8   6*   �� 5�       � ��$ Disabled=�6B    5�      �� 5�      # Z" 6:    6   �� $ MixDisplayMode�.a�5�      ��  � ��8  6,   �� 5�       � �� 5�      �� 5�   	   .R�5�   
   � 5�   
   5�      5�      5�      �� 5�      .R�5�      �� $	��150��6   $ Vertical=�6   #���  63   .+�5�      �� $��18��6   $��Center=�6   $��	Username:��63   �� 5�      $��txtUserName��6   $��24��6   $	��Top=�6-   # 6E   �� *IsUsernameFieldFocused+$ TwoWay=���6�   �� *UsernameField+$ TwoWay=�$$ PropertyChanged=��� 5�      $��18��6   $��Center=�6   $��	Password:��63   G� 5�      $% txtPassWord��6   $��24��6'   $	��Top=�63   & . # 6K   �� *IsPasswordFieldFocused+$ TwoWay=�' 6�   �� $ PasswordField�$ TwoWay=�$$ PropertyChanged=�( 5�      �� 5�      $) Enter��6-   * 6   �� *LogIn+6-   5�      ]� 5�      .+�5�      �� $��loginButton��66   $��Right=�6   $��Log in��6%   ��6I   �� *LogIn+�� 5�      $
! Hand��6   �� 5      -   5        $��http://8tracks.com/signup?�5      
��� ���.�6w   .�5  	   �� $��No account?��6   $��CharacterEllipsis=�6&   $L FCreate an account on 8tracks. Click to go to http://8tracks.com/signup��6G   5  	   5     5     5     5     �� 5     .R�5	     �� $ Vertical=�6   $��50��6'   #���  65   .+�5
     �� $ 
Horizontal=�6   .+�5     �� $	��Hi ��6   �� 5     ��6   �� $ UserData.Username�5     �� 5     $��logoutButton��60   $��Log out��6   $��0,5,0,0q�6   $��Right=�6d   ��6D   �� $ LogOut�6d   5     5     �� 5     .�5     ��  � �� 5     �� 5     " ��6   .R�5     �� ._�5  	   �� $��0q�6   $��0��6   
������ $	+ 1 2R�6G   5  	   5     5     5     �� 5     " 9�6
   8   .�5      ��  � ��$ Log in��6   �� 5!      � ��8  6'   �� 5"      �@�FocusVisualStyle# �  6(    5#     �� 5$      6   �� $ UserLoggedIn� true68   .a�5%     ��  � ��$ Profile��6    �� 5&      � ��8  6)   5'     5)     �� 5,  
   " ��6   8   .�5-     ��  � ��$ Download��6   �� 5.      � y� 6"   �� *DebugModeOn+$, Window�� 5/     �� 50      6   �� $ TrackLocation� Downloading69   .a�51     ��  � ��$ Downloading��6!   �� 52      � {� .  6#   53     �� 54      6   �� $ TrackLocation� 
Downloaded69   .a�55     ��  � ��$ Download��6!   �� 56      � {� . 6#   �� 57      � ��
 ��·�6$   58     5:     �� 5<     " ��6
    5=     �� 5>      6   �� *IsCurrentlyPlayed+ True68   .a�5?     ��  � ��$
 Bold"�6$   5@     �� 5B      6   �� *IsLast+ True6-   .a�5C     ��  �@�ToolTip$' !This is the last song of the mix.��6!   5D     5G     �� 5I     " ��6
   8    5J     �� 5K      6   �� *IsLast+ True6-   .a�5L     ��  � ��
 �  ��6$   5M     5O     �� 5Q     .R�5R     ]� ��. .+�5T     �� $��5,0,5,0q�6v   $��Right=�6�   8��  ��6   �� *
Download+��66   �� $ DebugModeButtonWidth�$, Window��6�   �� 5U     $��Right=�6   $��2q�6L   ��6/   �� *
Duration+6L   �� 5V     $ 
Horizontal=�6   .+�5W     �� $��20��6   8�� $
��Wrap=�6H   $��CharacterEllipsis=�6\   �� 5X     8�� ��6   �� $ 	Performer�61    �� 5Y     $	�� - ��6   8�� 6    �� 5Z     8�� ��6   �� $
 Name�6,   5[     5\     5]     5^     �� 5`     " ��6
   8   .�5a     ��  � ��8  6'   �� 5c      � ��$ Stretch=�62   5e     �� 5j     " ��6
   .�5l     ��  � ��8   6*   �� 5m      � ��$ Disabled=�6B   5n     5q     .��5t     � #���  6   $��Stretch=�6x   $��Stretch=�6�   |�.  6=   �� $, Window��$ 	ViewModel�- � ColumnDefinitions- 5u     �� 5v     $	��55* �6   $	��200��6"   �� 5w     $
��Auto �6   �� 5x     $	��45* �6   $	��200��6"   .+�5z     �� -   5        $��gridSplitter1��5z  j   $��Stretch=�6   $��Right=�6-   . �� ResizeBehavior$. PreviousAndNext=�6I   $��5��6   $��1��6�   #���  6�   ]� 5|     ��. $��0��6#   #���  63    6h   �� $ MediaBrowserViewModel�.+�5}     ]� $
��Auto��6   $��5,5,5,5q�6   $	��Top=�6.   ��.  .+�5~     �� $��2,0,2,0q�66   ��6   �� *GetRecentlyMadeMixes+.��5     �� .+�5�     �� $"��../Resources/RecentMixes.png��6   $��40��64   �� 5�     $��Recent��6   $��Center=�6   5�     5�     �� 5�     $��2,0,2,0q�6-   ��6   �� *GetHotMixes+.��5�     �� .+�5�     �� $��../Resources/HotMixes.png��6   $��40��61   $��40��6=   �� 5�     $	��Hot��6   $��Center=�6   5�     5�     �� 5�     $��2,0,2,0q�61   ��6   �� *GetPopularMixes+.��5�     �� .+�5�     �� $#��../Resources/PopularMixes.png��6   $��40��65   $��40��6A   �� 5�     $��Popular��6   $��Center=�6    5�     5�     �� 5�     $��2,0,2,0q�6/   ��6   �� *GetLikedMixes+.��5�     �� .+�5�     �� $!��../Resources/LikedMixes.png��6   $��40��63   $��40��6?   / J� Opacity$	/ 0.7Q�6J   �� 5�     $��Liked��6   $��Center=�6   5�     5�     �� 5�     $��2,0,2,0q�6.   ��6   �� *GetFeedMixes+.��5�     �� .+�5�     �� $ ��../Resources/FeedMixes.png��6   $��40��62   $��40��6>   �� 5�     $
��Feed��6   $��Center=�6   5�     5�     �� 5�     $��2,0,2,0q�6=   ��6   �� *GetHistoryMixes+.��5�     �� .+�5�     �� $!��../Resources/Headphones.png��6    $��40��6E   $��40��6Q   �� 5�     $��History��6    $��Center=�62   5�     5�     �� 5�     $��2,0,2,0q�6A   ��6   �� *GetRecommendedMixes+.��5�     �� .+�5�     �� $��../Resources/Stamp.png��6    $��40��6@   $��40��6L   �� 5�     $��	Suggested��6    $��Center=�64   5�     5�     �� 5�     #��Z" 6?   $��2,0,2,0q�6~   $��Right=�6�   ��6   �� *SetMixDisplayMode+.��5�     �� .+�5�     �� $$��../Resources/ThumbnailView.png��6   $��40��66   $��40��6B   �� 5�     $��Thumbs��6   $��Center=�6   5�     5�     �� 5�     0   Detailed#��Z0 63   $��2,0,2,0q�6t   $��Right=�6�   ��6   �� *SetMixDisplayMode+.��5�     �� .+�5�     �� $��../Resources/ListView.png��6   $��40��61   $��40��6=   �� 5�     $��Detailed��6   $��Center=�6!   5�     5�     5�     � 5�     $��5,5,5,5q�6
   $	��Top=�6   - 5�     �� 5�     $
��6.7* �6   �� 5�     $
��3.3* �6   .+�5�     �� $ 
Horizontal=�6   $��0,0,9,0q�6*   .+�5�     �� $��Filter��6   $��Center=�6   $��2,0,0,0q�69   � 5�     $��10,0,0,0q�6   $� �Put tags in square brackets. Example: [jazz][hiphop] covers => searches for mixes that contain "covers" and tagged with jazz and hiphop��6   $��1��6�   ���	#FF333333#��[ 6�   .+�5�     �� $	��0,2q�6   $	��250��6   $7��1 Filter the results. Put tags in square brackets.��6+   
�������y�5�  	   `� 5�  
   1 `� 	Converter#1 [ 6   .1�5�     �� $, txtUserEntry2��6   $ Text.IsEmpty�6/   �� 5�     $, txtUserEntry2��6   $ 	IsFocused�6/   5�     ( 5�     �� 5�  "   $) Enter��6M   * 6-   �� *ApplyFilter+6M   5�  	   �� 5�     -   5        $��txtUserEntry2��5�     $	��250��6%   
������ 
���   �2 �� 
CaretBrush
2 �������6q   �� *Filter+$ TwoWay=�$$ PropertyChanged=�.��6�   ( 5�     �� 5�  "   $) Enter��6M   * 6-   �� *ApplyFilter+6M   .��5�  5   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $��1��6*   $��Right=�6:   .+�5�     �� $��Number of mixes:��6   $��5,5,5,5q�6(   $��Center=�69   �� 5�     3 �� SelectedIndex$3 0��6   
�������$��45��6A   4 �� SelectedValuePath$4 Content��6�   5 �� SelectedValue5 6L   �� *MixesBatchSize+$ TwoWay=�.@�5�     �� $��10��6   �� 5�     $��20��6   �� 5�     $	��100��6   5�     5�     5�     �� 5�     $��Bottom=�6   $ 
Horizontal=�6(   $��5,5,5,5q�6A   .+�5�     �� $	��110��6.   $��30��6:   $��Right=�6F   $��5,0,10,0q�6]   ��6   �� *GetMoreMixes+.��5�     �� $ 
Horizontal=�6   .+�5�     �� $ ��../Resources/MoreMixes.png��6   $��25��62   $��Center=�6>   $��0,0,5,0q�6Y   �� 5�     $��
More mixes��6   $��Center=�6#   $��Center=�6@   5�     5�     �� 5�     $��Center=�6?   ��6   �� *MixBrowsingMessage+ 6Z   �� *MixBrowsingMessage+5�     �� 5�     -   5        $��	mixesList��5�  O   $
��Auto��6   #���  6   #��[ 6�   $	��Top=�6�   ��6`   �� *BrowsedMixes+6�   5�     ]� 5�     $��2��6=   #���  6M   ��.  6   �� $ PlaybackViewModel�.+�5�     ]� ��. $	��Top=�6$   .+�5�     �� $ Vertical=�6   $	��170��6(   .+�5�     �� 
�������$��2q�6!   $ 4��65   $��5q�6F   ._�5�     �� $
��Left=�6   $
��Left=�6$   $	��Top=�6?   $	��160��6�   $	��160��6�   ��6W   �� *CurrentMixViewModel.CoverUri+6�   5�     �� 5�     $ 
Horizontal=�6   $��5,0,0,0q�6+   .+�5�     �� $!��..\Resources\PlaysCount.png��6   $��16��63   $��16��6?   $��0,0,5,0q�6J   �� 5�     $��0,0,10,0q�6A   ��6   �� * CurrentMixViewModel.PlaysCount+6A   �� 5�     $!��../Resources/LikesCount.png��6   $��16��63   $��16��6?   $��0,0,5,0q�6J   �� 5�     $��0,0,10,0q�6A   ��6   �� * CurrentMixViewModel.LikesCount+6A   5�     �� 5�     $��CharacterEllipsis=�6>   $
��Wrap=�6_   
�������$��40��6�   $��5q�6�   ��6   �� *CurrentMixViewModel.TagList+ 6�   �� *CurrentMixViewModel.TagList+5�     �� 5�     $��Right=�6   
������  6(   �� $, Window��$" ViewModel.LoginFormViewModel�.+�5�     9� 6 9� ExpandDirection$6 Right=�6   $	��190��6(   $	��Top=�65   $��Right=�6M   #��[ 6�   #���  6  7 9� 
IsExpanded7 6i   �� *IsLoginFormExpanded+$ TwoWay=�$$ PropertyChanged=���6�   �� 6  5�     �� 5�     $ Vertical=�6   $
��Left=�6(   8 K� ZIndex$8 1��6C   $��5q�6T   $	��230��6_   $	��230��6l   .+�5      �� $
! Hand��6   �� 5     -   5        
������ ��5     �� * CurrentMixViewModel.RestfulUri+.�6�   .�5  	   �� $��22(�6p   $��CharacterEllipsis=�6~   $
��Wrap=�6�   
�������$��SemiBold"�6�   $	��170��6�   ��6   �� *CurrentMixViewModel.Name+ 6=   �� * CurrentMixViewModel.RestfulUri+6�   5  	   5     �� 5     $��16(�6<   ��6   �� *CurrentMixViewModel.Author+6<   �� 5     $��Italic$�6B   $��CharacterEllipsis=�6U   $
��Wrap=�6v   
���������6   �� *!CurrentMixViewModel.Description+6�   5     5     � 5     $��Stretch=�6
   $��5,5,5,5q�6(   $	��Top=�69   - 5     �� 5     $��* �6   �� 5     $	��180 �6   .+�5     �� $��0��6   $��Center=�6   $��0,0,15,0q�6f   9 �� IsMoveToPointEnabled9 . {�68   �� *IsPositionSliderEnabled+: � Value: 6�   �� *CurrentPosition+$ TwoWay=�; � Maximum; 6�   �� *CurrentTrackDuration+]� 5     $��1��6   ��. $��Center=�65   .+�5     �� $
��Left=�69   $��Center=�6O   ��6   �� *CurrentPositionAsString+6O   �� 5     $��/��6   $
��Left=�6   $��Center=�6/   �� 5     $
��Left=�6>   $��Center=�6T   ��6   �� *CurrentTrackDurationAsString+6T   �� 5     $��../Resources/Volume.png��6   $
��Left=�6.   $��20��6D   $��25��6P   $��0,0,3,0q�6[   �� 5     $��Stretch=�6   $��Center=�6,   $��Right=�6G   $	; 100Q�6^   < � SmallChange$< 2Q�6l   = � LargeChange$= 10Q�6|   9 . : 6�   �� *Volume+$ TwoWay=�6�   5     5     ]� 5     $��5,5,5,5q�6   ��.  $	��Top=�66   .+�5     �� $��2,2,0,2q�6*   ��6   �� *
Continue+.��5     �� $��../Resources/Play.png��6   $��25��6,   $��25��68   5     �� 5     $��2,2,0,2q�6'   ��6   �� *Pause+.��5      �� $��../Resources/Pause.png��6   $��25��6-   $��25��69   5!     �� 5#     $��2,2,0,2q�6&   ��6   �� *Stop+.��5$     �� $��../Resources/Stop.png��6   $��25��6,   $��25��68   5%     �� 5&     $��2,2,0,2q�6*   ��6   �� *
NextSong+.��5'     �� $ ��../Resources/NextTrack.png��6   $��25��61   $��25��6=   5(     �� 5*     $��2,2,2,2q�6)   $��Right=�6:   ��6   �� *	NextMix+.��5+     �� $ 
Horizontal=�6   .+�5,     �� $��../Resources/NextMix.png��6   $��25��60   $��25��6<   �� 5-     $��Next Mix��6   $��Center=�6!   $��3,0,3,0q�6<   5.     5/     �� 51     $ 
Horizontal=�6   $��Right=�66   $��	10,0,10,0q�6M   .+�52     �� $��Track ��6   $��Center=�6-   �� 53     $��Center=�6^   ��6   �� */-CurrentMixViewModel.CurrentTrackIndexAsString+6^   �� 54     $
�� of ��6   $��Center=�6+   �� 55     $��Center=�6X   ��6   �� *)'CurrentMixViewModel.TotalNumberOfTracks+6X   �� 56     $�� (��6   $��Center=�6)   �� 57     $��Center=�6Z   ��6   �� *+)CurrentMixViewModel.TotalDurationAsString+6Z   �� 58     $��)��6   $��Center=�6(   59     5:     ]� 5<     $��Bottom=�6   ��.  $��4q�6F   .+�5>     �� $��2,2,2,2q�6L   $
��Left=�6]   ��6   �� * CurrentMixViewModel.ToggleLike+.��5?     �� $ 
Horizontal=�6    6+   �� *CurrentMixViewModel+.+�5@     �� #��[ 6   $��24��69   $��24��6E   �� 5A     $��Toggle like��6   $��Center=�6$   $��5,0,3,0q�6?   5B     5C     �� 5E     $��2,2,2,2q�6�   $
��Left=�6�   ��6   �� *CurrentMixViewModel.Download+y�6J   �� *DebugModeOn+$, Window��.��5F     �� $ 
Horizontal=�6!   .+�5G     �� $��../Resources/Record.png��6    $��25��6A   $��25��6M   �� 5H     $��Download all��6    $��Center=�67   $��3,0,3,0q�6R   5I     5J     �� 5L     $
! Hand��6   $��Right=�6*   $��Center=�6A   $��0,0,5,0q�6\   �� 5M  
   -   5        
�������.�5M  ?   .�5N     �� $��About...��6    $C =Click to see the information about the played and its author.��63   5O     5P     �� 5R     $
! Hand��6   $��Right=�6*   $��Center=�6A   $��0,0,5,0q�6\   �� 5S  
   -	   5        $7��1mailto://matija32@gmail.com?subject=[8tracks App]?�5S     
��� ���.�6�   .�5T     �� $��Bugs? Suggestions?��6    $I CSend an email to the author of the application - matija32@gmail.com��6=   
��� ���6�   5U     5V     5X     �� 5Z     $��5,0,5,0q�6   #���  6   #��[ 6�   ��6S   �� *CurrentMixViewModel.Tracks+>   8EightTracksPlayer.Utils.ViewExtensions.ListBoxExtensions>   FocusedItem> 6�   �� *'%CurrentMixViewModel.CurrentTrackIndex+3 6  �� *SelectedTrackIndex+( 5\     �� 5]     $) Enter��60   * 6   �� *	GoToTrack+60   .  (System.Windows.Interactivity.Interaction?   Triggers? 5_     /  )System.Windows.Interactivity.EventTrigger  5`     @   	EventName$@ MouseDoubleClick��6   .  (System.Windows.Interactivity.TriggerBaseA   Actions.A 5a     6	  0System.Windows.Interactivity.InvokeCommandAction	  B 	  CommandB 6   �� *	GoToTrack+5b     5e     5g     ( 5h     �� 5i     C �� Gesture$C CTRL+1��6?   * 6   �� *PlaybackViewModel.Continue+6?   �� 5j     $C CTRL+2��6<   * 6   �� *PlaybackViewModel.Pause+6<   �� 5k     $C CTRL+3��6;   * 6   �� *PlaybackViewModel.Stop+6;   �� 5l     $C CTRL+4��6?   * 6   �� *PlaybackViewModel.NextSong+6?   �� 5m     $C CTRL+9��6>   * 6   �� *PlaybackViewModel.NextMix+6>   �� 5o     $C CTRL+R��6X   * 6   �� *,*MediaBrowserViewModel.GetRecentlyMadeMixes+6X   �� 5p     $C CTRL+H��6O   * 6   �� *#!MediaBrowserViewModel.GetHotMixes+6O   �� 5q     $C CTRL+P��6S   * 6   �� *'%MediaBrowserViewModel.GetPopularMixes+6S   �� 5r     $C CTRL+L��6Q   * 6   �� *%#MediaBrowserViewModel.GetLikedMixes+6Q   �� 5s     $C CTRL+F��6P   * 6   �� *$"MediaBrowserViewModel.GetFeedMixes+6P   �� 5t     $C 	CTRL+Plus��6P   * 6   �� *$"MediaBrowserViewModel.GetMoreMixes+6P   �� 5v     D �� CommandParameter$D 5��6;   $C 
CTRL+Right��6P   * 6   �� *PlaybackViewModel.Skip+6P   �� 5w     $D -5��6;   $C 	CTRL+Left��6Q   * 6   �� *PlaybackViewModel.Skip+6Q   �� 5x     $D 3��6C   $C CTRL+Up��6X   * 6   �� * PlaybackViewModel.ChangeVolume+6X   �� 5y     $D -3��6C   $C 	CTRL+Down��6Y   * 6   �� * PlaybackViewModel.ChangeVolume+6Y   5~     5     