<?php

$xml = file_get_contents("http://ws.nasdaqdod.com/v1/NASDAQAnalytics.asmx/GetEndOfDayData?Symbols=" . $_GET['ticker'] . "&StartDate=11/10/16&EndDate=11/11/16&_token=BC2B181CF93B441D8C6342120EB0C971&MarketCenters=Q,N");

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

function encrypt($text){
	$key = "b6 a0 26 45 97 43 ff 74 27 e9 6e 85 3e cd 18 7c 4c 98 56 12 ff 70 ab 75 25 bd 32 54 90 1a b1 7e";
	$IV = mcrypt_create_iv(mcrypt_get_iv_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC), MCRYPT_RAND); 
	return base64_encode(mcrypt_encrypt(MCRYPT_RIJNDAEL_256, $key, $text, MCRYPT_MODE_CBC, $IV)."-[--IV-[-".$IV); 
}


$parsed = new SimpleXMLElement($xml);
$prev = floatval($parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[0]->Close);
$val = floatval($prev - $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Close);
$str = "";

if($val > 0){
	$str .= "G ";
	$str .= "+" . round($val,2) . "(+".round(($val/$prev)*100, 2)."%) ";
} else {
	$str .= "R "; 
	$str .= round($val,2) . "(".round(($val/$prev)*100, 2)."%) ";
}
$str .= $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Open . " ";
$str .= $parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[1]->Close . " ";
$str .= nice_number($parsed->EndOfDayPriceCollection[0]->Prices[0]->EndOfDayPrice[0]->Volume) . " ";

if(isset($_GET['encrypt'])){
	echo encrypt($str);
} else {
	echo $str;
}

?>