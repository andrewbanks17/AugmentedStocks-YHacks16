<?php



$xml = file_get_contents("http://ws.nasdaqdod.com/v1/NASDAQAnalytics.asmx/GetEndOfDayData?Symbols=" . $_GET['ticker'] . "&StartDate=11/10/16&EndDate=11/11/16&_token=BC2B181CF93B441D8C6342120EB0C971&MarketCenters=Q");

function nice_number($n) {
	// first strip any formatting;
	$n = (0+str_replace(",", "", $n));
// is this a number?
	if (!is_numeric($n)) return false;

	// now filter it;
	if ($n > 1000000000000) return round(($n/1000000000000), 2).'T';
	elseif ($n > 1000000000) return round(($n/1000000000), 2).'B';
	elseif ($n > 1000000) return round(($n/1000000), 2).'M';
	elseif ($n > 1000) return round(($n/1000), 2);

	return number_format($n);
}
$parsed = new SimpleXMLElement($xml);
$prev = floatval($parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[0]->Close);

$val = floatval($prev - $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Close);
if($val > 0){
	echo "G ";
	echo "+" . round($val,2) . "(+".round(($val/$prev)*100, 2)."%) ";
} else {
	echo "R "; 
	echo round($val,2) . "(".round(($val/$prev)*100, 2)."%) ";
}
echo $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Open . " ";
echo $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Close . " ";
echo nice_number($parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[0]->Volume) . " ";

?>