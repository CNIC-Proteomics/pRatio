### params
deltaMassThreshold = 15 # in ppm
deltaMassAreas = 5 # number of jumps: 1,3 or 5
input="/Users/proteomica/Documents/pRatio/TMT1_PESA.msf"
output="/Users/proteomica/Documents/pRatio/TMT1_PESA_1.xls"

###
#install.packages("installr")
#install.packages("RSQLite")
#install.packages("readr")
#install.packages("stringi")
#install.packages("plyr")
#install.packages("Peptides")

#library(stringi)
#library(readr)
#library(RSQLite)
#library(plyr)
#library(Peptides)

db=dbConnect(SQLite(), dbname=input)

queryMain = "select 
p.peptideid, 
fi.filename, 
sh.firstscan, 
sh.lastscan, 
sh.charge, 
p.sequence,
sh.mass,  
ps.scorevalue,  
sh.retentiontime,  
p.searchenginerank, 
p.deltascore  
from peptides p, 
peptideScores ps, 
spectrumHeaders sh, 
massPeaks mp, 
workFlowInputFiles fi, 
processingNodeScores scoreNames 
where p.peptideid = ps.peptideid 
and sh.spectrumid = p.spectrumid 
and (fi.fileid = mp.fileid or mp.fileid = -1) 
and mp.masspeakid = sh.masspeakid 
and scoreNames.scoreid = ps.scoreid 
and scoreNames.ScoreName = 'Xcorr'  
and p.searchenginerank = 1   

order by 
fi.filename desc,  
sh.firstscan asc, 
sh.lastscan asc,  
sh.charge asc,  
ps.scorevalue desc" 

data=dbGetQuery(conn = db, queryMain)

queryModifications = "select 
p.peptideid, 
paam.aminoacidmodificationid, 
paam.position,  
p.sequence,  
aam.modificationname,  
aam.deltamass   
from peptides p, 
spectrumHeaders sh, 
peptidesaminoacidmodifications paam, 
aminoacidmodifications aam 
where p.peptideid = paam.peptideid 
and sh.spectrumid = p.spectrumid 
and aam.aminoacidmodificationid = paam.aminoacidmodificationid 
and p.searchenginerank = 1 
order by p.peptideid ASC, paam.position ASC"

dataMod=dbGetQuery(conn = db, queryModifications)


queryProteinInfo = "select 
pq.peptideid, 
p.sequence, 
pq.proteinid,  
q.description  
from peptidesProteins pq, 
spectrumHeaders sh, 
peptides p, 
proteinAnnotations q 
where pq.peptideid = p.peptideid 
and pq.proteinid = q.proteinid 
and sh.spectrumid = p.spectrumid 
and p.searchenginerank = 1 
order by pq.peptideid asc"

dataProt=dbGetQuery(conn = db, queryProteinInfo)

## prepare DATA: mods & prots
ptmAnnotation <- function(x)
{
  pep<-x[1,]$Sequence
  b<-0
  p<-""
  modNum<-1
  modMass<-0
  for (i in x$Position)
  {
    p <- stri_flatten(c(p,substr(pep,b,i+1),"[",x[modNum,]$DeltaMass,"]"),collapse="")
    modMass<-modMass + x[modNum,]$DeltaMass
    #p <- paste(p,substr(pep,b,i+1),"[",x[modNum,]$DeltaMass,"]",sep="")
    b <- i+2
    modNum <- modNum+1
  }
  #p <- paste(p,substr(pep,b,nchar(pep)),sep="")
  p <- stri_flatten(c(p,substr(pep,b,nchar(pep))),collapse="")
  return(c(p,modMass))
}
dataModAnnotation <- ddply(dataMod,.(PeptideID),ptmAnnotation)
colnames(dataModAnnotation) <- c("PeptideID","Sequence","modMass")
dataModAnnotation$modMass <- as.numeric(dataModAnnotation$modMass)

# mix unmodified and modified
dataModTmp <- merge(unique(data[,c("PeptideID","Sequence")]),dataModAnnotation,by = "PeptideID",all.x=TRUE)
dataModTmp[is.na(dataModTmp["Sequence.y"]),"Sequence.y"] <- dataModTmp[is.na(dataModTmp["Sequence.y"]),"Sequence.x"]
dataModAll <- dataModTmp[,c("PeptideID","Sequence.y","modMass")]
colnames(dataModAll) <- c("PeptideID","SequenceMod","modMass")

redundances <- aggregate(Description ~ PeptideID, data=dataProt, paste, collapse = " -- ")
colnames(redundances) <- c("PeptideID","Redundances")
dataProt.u <- dataProt[!duplicated(dataProt["PeptideID"]),]
peptideProt <- merge(dataProt.u[,c("PeptideID","Description")], redundances, by="PeptideID", all.x=TRUE)

#*****
dataAll <- cbind(data, dataModAll$"SequenceMod",  dataModAll$"modMass", peptideProt[ , -which(names(peptideProt) %in% c("PeptideID"))])
colnames(dataAll) <- c("PeptideID","FileName","FirstScan","LastScan","Charge","Sequence","Mass","ScoreValue","RetentionTime","SearchEngineRank","DeltaScore","SequenceMod","modMass","Description","Redundances")

## Calculate theoretical mass
dataAll[is.na(dataAll[,"modMass"]),]$modMass <- 0
dataAll <- cbind(dataAll,as.data.frame(unlist(lapply(dataAll[,c("Sequence")], mw, monoisotopic=TRUE))))
names(dataAll)[length(names(dataAll))]<-"Theoretical" 
dataAll$Theoretical <- dataAll$Theoretical + 1.00727647
dataAll <- cbind(dataAll, dataAll$Theoretical + dataAll$modMass + 229.162932)
names(dataAll)[length(names(dataAll))]<-"TheoreticalModTag" 
dataAll <- cbind(dataAll, abs(dataAll$Mass - dataAll$Theoretical - dataAll$modMass - 229.162932) / dataAll$Mass * 1e6)
names(dataAll)[length(names(dataAll))]<-"deltaMassTargetppm" 

## filter by xCorr > 1.5 to speedup process
dataAll <- subset(dataAll,ScoreValue>1.5)

## Decoy tagging
decoy_tag = "_INV_"
isDecoy <- rep(0, dim(dataAll)[1])
isTarget <- rep(1, dim(dataAll)[1])
protein <- dataAll[,'Description']
index <- grep(decoy_tag,protein,fixed=TRUE)
isDecoy[index] <- 1
isTarget[index] <- 0
dataAll <- cbind(dataAll,isDecoy,isTarget)

## filter by deltaMass
filterDeltaMass <- function(x, deltaMassThreshold, deltaMassAreas)
{
  TheoreticalModTag=x[1]
  Mass=x[2]
  ScoreValue=x[3]
  jump1_ppm = abs(TheoreticalModTag - Mass) / TheoreticalModTag * 1e6
  if (jump1_ppm >= deltaMassThreshold)
  {
    if (deltaMassAreas <= 1) { return(0.01) } # jump 1 >= threshold
    else
    {
      MassCorr <- Mass - 1.0033
      jump23_ppm = abs(TheoreticalModTag - MassCorr) / TheoreticalModTag * 1e6
      if (jump23_ppm >= deltaMassThreshold)
      {  
        if (deltaMassAreas <= 3) { return(0.01) } # jump 23 >= threshold
        else
        {
          MassCorr2 <- Mass - 1.0033
          jump45_ppm = abs(TheoreticalModTag - MassCorr2) / TheoreticalModTag * 1e6
          if (jump45_ppm >= deltaMassThreshold) {return (0.01)} # jump 45 >= threshold
          else {return (ScoreValue)} # jump 45 < threshold
        }
      }
      else
      {
        return (ScoreValue) # jump 23 < threshold
      }
    }
  }
  else
  {
    return(ScoreValue) # jump 1 < threshold
  }
}
jump1ScoreValue <-as.data.frame(unlist(apply(dataAll[,c("TheoreticalModTag","Mass","ScoreValue")], 1, filterDeltaMass, deltaMassThreshold=deltaMassThreshold, deltaMassAreas=deltaMassAreas)))
colnames(jump1ScoreValue) <- "ScoreValueAfterJUMP"
dataAll$ScoreValue<-jump1ScoreValue$ScoreValueAfterJUMP

## Add xcorr_c
n = dim(dataAll)[1]

xcorr_c <- function(x) {
  r=1
  if(as.numeric(x[1])>2) {r=1.22}
  xcorr_c = log((as.numeric(x[2]))/r)/log(2*nchar(as.character(x[3])))
  return (xcorr_c)
}

dataAll <- cbind(dataAll,apply(dataAll[,c("Charge","ScoreValue","Sequence")], 1, xcorr_c))
colnames(dataAll)[ncol(dataAll)] <- "xcorr_c"


# sort by xcorr_c
dataAll <- dataAll[order(decreasing = TRUE,dataAll$xcorr_c),]
tmp <- cbind(dataAll[, "xcorr_c"], dataAll[, "isDecoy"])
FP <- cumsum(tmp[, 2])
tmp <- cbind(tmp, FP)
xcorr_cP <- unlist(lapply(1:n, function(x) (tmp[x, 'FP'])/n))
dataAll <- cbind(dataAll, xcorr_cP)

### FDR CALC
dataAll <- dataAll[order(decreasing = FALSE,dataAll$xcorr_cP),]
tmp <- cbind(dataAll[, "xcorr_cP"], dataAll[, "isDecoy"], dataAll[, "isTarget"])
FP <- cumsum(tmp[, 2])
TP <- cumsum(tmp[, 3])
tmp <- cbind(tmp, FP, TP)
xcorr_cp_FDR <- unlist(lapply(1:dim(dataAll)[1], function(x) (tmp[x, 'FP'])/tmp[x, 'TP']))
dataAll <- cbind(dataAll, xcorr_cp_FDR)

res <- dataAll[dataAll$xcorr_cp_FDR < 0.01 & dataAll$isTarget == 1,]

fileName <- strsplit(data[1,"FileName"], fixed = TRUE, split = "\\")[[1]][length(strsplit(data[1,"FileName"], fixed = TRUE, split = "\\")[[1]])]
pRatio <- "NA"; pI <- "NA"; Xcorr1Original <- "NA"; Xcorr2Search <- "NA"; Sp <- "NA"; SpRank <- "NA"; ProteinsWithPeptide <- "NA"

resPratio <- cbind(fileName,fileName,res[,c("FirstScan","LastScan","Charge")],pRatio,res[,c("xcorr_cp_FDR","Description","SequenceMod")],pI,res[,c("Mass","xcorr_c")],Xcorr1Original,Xcorr2Search,res[,"DeltaScore"],Sp,SpRank,ProteinsWithPeptide,res[,"Redundances"])

colnames(resPratio) <- c("FileName","RAWFile","FirstScan","LastScan","Charge","pRatio","FDR","FASTAProteinDescription","Sequence","pI","PrecursorMass","Xcorr1Search","Xcorr1Original","Xcorr2Search","DeltaCn","Sp","SpRank","ProteinsWithPeptide","Redundances")

# pRatio modification parsing
resPratio$Sequence <- gsub('\\[57.021464\\]','*',resPratio$Sequence)
resPratio$Sequence <- gsub('\\[15.994915\\]','#',resPratio$Sequence)
resPratio$Sequence <- gsub('\\[229.162932\\]','@',resPratio$Sequence)
resPratio$Sequence <- gsub('\\[113.08407\\]','^',resPratio$Sequence)

write.table(resPratio,file = output,col.names = TRUE, row.names = FALSE,sep="\t", quote = FALSE)


  

