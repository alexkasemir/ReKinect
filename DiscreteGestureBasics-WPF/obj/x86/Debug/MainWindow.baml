   M S B A M L   `   `   `  ����T  PDiscreteGestureBasics-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics.Microsoft.Samples.Kinect.DiscreteGestureBasics  5   	   ?    9Microsoft.Samples.Kinect.DiscreteGestureBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Nlocal<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics     5   	   -   5          � Title$  Discrete Gesture Basics��5   	   $	��650��5   	   $	��750��6    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� DataTypeF   @Microsoft.Samples.Kinect.DiscreteGestureBasics.GestureResultView � RowDefinitions �� StringFormat��%�  &	        &	 #     &	 F     (�� i     5      *, +)�� 5	   
   $ 	#ff6e6e6e��62   �� 5
   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   "  6   .R�5      � $
��Auto��6   $��5q�6    ��6+   �� *	BodyColor+ 5      �� 5      $
��Auto �6$   �� 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��18(�6c   $��Segoe UI)�6q   $��SemiBold"�6�   $��1��6�   $��5 0 0 0q�6�   ��6   �� *Overall_Score+$ Your Overall Score: {0}��6�   �� 5      $��18(�6~   $��Segoe UI)�6�   $��SemiBold"�6�   $��2��6�   $��5 5 0 0q�6�   ��6   �� *Percent_TouchingFace+$1 +Percentage of Time Spent Touching Face: {0}��6�   �� 5      $��18(�6s   $��Segoe UI)�6�   $��SemiBold"�6�   $��3��6�   $��5 0 0 0q�6�   ��6   �� *Percent_Leaning+$+ %Percentage of Time Spent Leaning: {0}��6�   5      5      5      .��5       � $��	10 0 10 0q�6    5!   
   �� 5"      $
��Auto �6   �� 5#      $��* �6   �� 5$      $
��Auto �6   .+�5'   
   � -   5         �Name$ contentGrid��5'      $��1��6$   $��-10,0,-10,0q�61   $��2��6F   
�������$
��Left=�6j   $	��745��6�    � ColumnDefinitions 5(      �� 5)      $��1* �6#    5+      �� 5,      5/      �� 51   
   -   5        $��	statusBar��51   ?   $��2��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�52      �� ��6   �� *
StatusText+53      54      55      