// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef _WIN32
#define _CRT_SECURE_NO_WARNINGS
#include "targetver.h"
#endif

#include <stdio.h>

#ifdef _WIN32
#include <tchar.h>
#endif

#include <vector>
#include <random>
#include <iomanip>

// CERN ROOT includes
#include <TApplication.h>
#include <TF1.h>
#include <TRandom3.h>
#include <TCanvas.h>
#include <TDiamond.h>
#include <TString.h>
#include <TGraph.h>
#include <TCanvas.h>
#include <TH1D.h>
#include <TMath.h>