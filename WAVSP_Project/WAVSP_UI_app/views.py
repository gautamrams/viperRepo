# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.shortcuts import render

from .models import wavspdb

from django.http import HttpResponseRedirect

from subprocess import Popen

import os

import time

import re

from urlparse import urlsplit

import multiprocessing

from multiprocessing import Process,Pool

import itertools

# Create your views here.

tool_cmd = [
["xsser --all=http:// ",""],
["nmap -p80 --script http-csrf.nse ",""],
["sslyze --heartbleed ",""],
["nmap -p443 --script ssl-poodle -Pn ",""],
["yes | dotdotpwn.pl -m http -h ",""],
["nmap -F --open -Pn ",""],
["golismero -e dns_malware scan ",""],
["nmap -p445,137-139 --open -Pn ",""],
["nmap -p161 -sU --open -Pn ",""],
["nmap --script http-slowloris --max-parallelism 400 -Pn ",""]
]

tool_status = [
["Found the following stored XSS vulnerabilities:", 0, "", "w", "xsser",["Failed to resolve"]],
["Found the following CSRF vulnerabilities:",0,"","w","nmapcsrf",["Found the following CSRF vulnerabilities:"]],
["VULNERABLE",0,"","w","hbleed",["Failed to resolve"]],
["VULNERABLE",0,"","w","sensitive",["Failed to resolve"]],
["Total Traversals found: 0",1,"","w","dotdotpwn",["Total Traversals found: 0", "Fuzz testing aborted"]],
["tcp open",0,"","n","nmapopen",["Failed to resolve"]],
["No vulnerabilities found",1,"","n","golism1",["Cannot resolve domain name","No vulnerabilities found"]],
["open",0,"","n","nmapsmb",["Failed to resolve"]],   
["open",0,"","n","nmapsnmp",["Failed to resolve"]],
["VULNERABLE",0,"","n","nmapdos",["Failed to resolve"]]
]

tool_count = 10

def home(request):
	all_tests = wavspdb.objects.all()
	all_tests.update(result='n')
	for i in range(tool_count):
		tool = wavspdb.objects.get(roll = i+1)
		if(tool_status[i][3] == 'n'):
			tool.vuln_type = "network"
			tool.save()
		else:
			tool.vuln_type = "web"
			tool.save()
		temp_file = "temp_"+tool_status[i][4]
		try:
			os.remove(temp_file)
		except OSError:
			pass
	return render(request,'home.html', {'all_tests':all_tests})

def starttest(request):
	url = request.POST.get('inputurl')
	numbers = range(tool_count)
	pool = Pool()
	target_url = url_maker(url)
	pool.map(merger, itertools.izip(numbers, itertools.repeat(target_url)))
	all_tests = wavspdb.objects.all()
	return render(request,'home.html', {'all_tests':all_tests})

def merger(arg1_arg2):
	return vulnerabilitycheck(*arg1_arg2)

def url_maker(url):
    if not re.match(r'http(s?)\:', url):
        url = 'http://' + url
    parsed = urlsplit(url)
    host = parsed.netloc
    if host.startswith('www.'):
        host = host[4:]
    return host

def vulnerabilitycheck(number,target):
	temp_file = "temp_"+tool_status[number][4]
	status = False
	cmd = tool_cmd[number][0] + target + tool_cmd[number][1] + " > " + temp_file + " 2>&1"
	try:
		pr = Popen(cmd , shell=True)
		pr.wait()
	except:
		pass
	output_file = open(temp_file).read()
	tool = wavspdb.objects.get(roll = number+1)
	if tool_status[number][1] == 0:
		if tool_status[number][0].lower() in output_file.lower():
			tool.result = 'f'
			tool.save()
		else:
			tool.result = 'p'
			tool.save()
	else:
		if any(i in output_file for i in tool_status[number][5]):
			tool.result = 'p'
			tool.save()
		else:
			tool.result = 'f'
		   	tool.save()

def report(request,report_id):
	temp_file = "temp_"+tool_status[int(report_id)-1][4]
	tool = wavspdb.objects.get(roll = report_id)
	output_file = open(temp_file).read()
	return render(request,'report.html',{'output_file':output_file,'name':tool.name,'result':tool.result})