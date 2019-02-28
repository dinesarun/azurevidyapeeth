val textFile = sc.textFile("D:\\GIT\\azurevidyapeeth\\Nov20-Spark_Deployment_Types\\Data\\NASA_Access_Log")
val counts = textFile.flatMap(line => line.split(" ")).map(word => (word, 1)).reduceByKey(_+_)
counts.collect().take(3).foreach(println)
counts.saveAsTextFile("D:\\GIT\\azurevidyapeeth\\Nov20-Spark_Deployment_Types\\Data\\NASA_Access_Log_WordCount")