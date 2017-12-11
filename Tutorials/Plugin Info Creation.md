# Plugin Info Creation Tutorial

### Why You Should Make One
As with anything you need to ask yourself; "Why should I do this when I don't need to?". Well, think of other developers for one,
what if your plugin hooks a function that another plugin hooks and does things, without the source to your plugin the other developer has no clue what's
going on, it can also help end users determine if you have any dependancies that need to be resolved before yours will work to it's full extent.

#### Layout
```{
  "Name": "Plugin Name",
  "Description": "A Template info.JSON",
  "Injection_Addresses": [ 0, 0, 0 ],
  "Depends": [ "Nothing" ]
}
```

Currently, Launch_LDR will look at the Depends section and throw a *WARNING* that the file doesn't exist, but will not stop the game from launching.
You should pull the whole DLL name in extension included for this to work properly.


They should be called ``PluginName_info.json``
