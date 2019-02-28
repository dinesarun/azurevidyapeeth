//WordCount: Normal wordcount program from input 'WarPeace.txt'
val textFile = sc.textFile("/Data/WarPeace.txt")
	
val counts = textFile.flatMap(line => line.split(" ")).map(word => (word, 1)).reduceByKey(_ + _)
val counts1 = counts.collect().map(m => m.toString().tail.init)
counts1.foreach(println)
sc.parallelize(counts1).saveAsTextFile("/Data/Output/Spark/WarPeaceCount")