import { Component, OnInit } from '@angular/core';
import { BackgroundJobsClient, DatabaseDto, DatabasesClient } from '../web-api-client';
import { provideIcons } from '@ng-icons/core';
import { ICONS } from '../common/icon';
import { ToastService } from '../common/toast/toast.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
  viewProviders: provideIcons(ICONS)
})
export class ReportsComponent implements OnInit {

  constructor(private client: DatabasesClient, private backgroundJobsClient: BackgroundJobsClient, private toastService: ToastService) {}

  ngOnInit(): void {
    
  }

}
