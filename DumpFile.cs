//< ItemsControl ItemsSource = "{Binding Path=Player.Cards, UpdateSourceTrigger=PropertyChanged}" Grid.Column = "1" Grid.ColumnSpan = "10" Grid.Row = "3" >
       
//                   < ItemsControl.ItemsPanel >
       
//                       < ItemsPanelTemplate >
       
//                           < StackPanel Orientation = "Horizontal" />
        
//                        </ ItemsPanelTemplate >
        
//                    </ ItemsControl.ItemsPanel >
        

//                    < ItemsControl.ItemTemplate >
        
//                        < DataTemplate >
        
//                            < Image Width = "120" Grid.Row = "{Binding CNumber}" >
           
//                                   < Image.Source >
           
//                                       < BitmapImage UriSource = "{Binding ImgUrl}" />
            
//                                    </ Image.Source >
            
//                                </ Image >
            
//                            </ DataTemplate >
            
//                        </ ItemsControl.ItemTemplate >
            
//                    </ ItemsControl >