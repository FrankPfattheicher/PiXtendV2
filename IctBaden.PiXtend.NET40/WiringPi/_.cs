
// This work is based on the wrapper class written by Daniel J Riches for Gordon Hendersons WiringPi C library

// included here to reduce more dependencies

// refactured for to be more Csharpy, better interop and smaller files


/************************************************************************************************
 * This wrapper class was written by Daniel J Riches for Gordon Hendersons WiringPi C library   *
 * I take no responsibility for this wrapper class providing proper functionality and give no   *
 * warranty of any kind, nor it's use or fitness for any purpose. You use this wrapper at your  *
 * own risk.                                                                                    *
 *                                                                                              *
 * This code is released as Open Source under GNU GPL license, please ensure that you have a    *
 * copy of the license and understand the usage terms and conditions.                           *
 *                                                                                              *
 * I take no credit for the underlying functionality that this wrapper provides.                *
 * Authored: 29/04/2013                                                                         *
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change
 * 08 May 2013  Daniel Riches       Corrected c library mappings for I2C and SPI, added this header
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 23 Nov 2013  Gerhard de Clercq   Changed digitalread to return int and implemented wiringPiISR
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 18 Jan 2016  Marcus Lum          Updated imported methods to current wiringPi 
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 05 Jan 2017  Ilmar Kruis         Added PullUp/Down enum 
 *
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change
 * 14 Sep 2017  Daniel Riches       Added softTone support, tested with GPIO 18 only so far
 *
 ************************************************************************************************/

