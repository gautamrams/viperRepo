# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models

# Create your models here.

class wavspdb(models.Model):
	roll = models.IntegerField(default = 1)
	name = models.CharField(max_length = 200)
	result = models.CharField(max_length = 20 , default = 'n')
	vuln_type = models.CharField(max_length = 200 , default = 'web')
