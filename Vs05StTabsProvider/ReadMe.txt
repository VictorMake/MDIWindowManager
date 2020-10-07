VS2005-Style Tabs Provider for MDIWindowManager ReadMe

1. In your project, add a reference to Vs05StTabsProvider.dll

2. Add the following code to the New procedure of your MDI form: 
Me.WindowManagerPanel1.CustomTabsProviderType = GetType
(Vs05StTabsProvider.Vs05StTabsProvider)

3. You may need to adjust the size WindowManagerPanel to have the 
tabs display properly

Please note: This is an *alternate* tabs provider for 
MDIWindowManager. MDIWindowManager already contains built-in robust 
tabs that are capable of displaying several styles as well as expose 
a TabPaint event where you can completely or partially handle the 
drawing of tabs.

*VS05StTabsProvider is based on code for the Tabstrip control written 
by H. Eskandari http://www.codeproject.com/cs/miscctrl/TabStrip.asp