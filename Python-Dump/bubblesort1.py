listArray = [7,1,3,4]

TempStorage = ""
while True:
  input()
  for x in range(0,len(listArray)):
    print(str(listArray) + " Working on pos"+str(x))
    #print (str(listArray[x])+", "+str(listArray[x+1]))

    try:
      if(listArray[x] > listArray[x+1] ):
        print("Working swapping " +str(listArray[x+1]) +","+str(listArray[x]))
        TempStorage = (listArray[x])
        listArray[x] = (listArray[x+1])
        listArray[x+1] = int(TempStorage)
    except IndexError:
      print("Hit a unexpected error with the array")
      break

