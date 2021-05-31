var db = window.openDatabase('PShop', 1.0, 'PDBPShop', 2*1024*1024);
db.transaction(function (tx) { 
   tx.executeSql('CREATE TABLE IF NOT EXISTS buylist ( Id  PRIMARY KEY   , BuyName   )',[],function(){}, function(ex,ey){ alert(ey.message);} );

 });
