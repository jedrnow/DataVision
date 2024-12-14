import { BrowserModule } from '@angular/platform-browser';
import { APP_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DatabasesComponent } from './databases/databases.component';
import { NgIcon } from '@ng-icons/core';
import { PaginationComponent } from './common/pagination/pagination.component';
import { AddDatabaseComponent } from './databases/add-database/add-database.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { DatabaseDetailsComponent } from './databases/database-details/database-details.component';
import { CommonModule } from '@angular/common';
import { EditDatabaseComponent } from './databases/edit-database/edit-database.component';
import { BrowseDatabaseComponent } from './databases/browse-database/browse-database.component';
import { ReportsComponent } from './reports/reports.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        DatabasesComponent,
        PaginationComponent,
        AddDatabaseComponent,
        DatabaseDetailsComponent,
        EditDatabaseComponent,
        BrowseDatabaseComponent,
        ReportsComponent
    ],
    bootstrap: [AppComponent],
    imports: [
        NgIcon,
        CommonModule,
        BrowserModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'databases/add', component: AddDatabaseComponent },
            { path: 'databases', component: DatabasesComponent },
            { path: 'databases/:databaseId', component: DatabaseDetailsComponent },
            { path: 'databases/:databaseId/edit', component: EditDatabaseComponent },
            { path: 'databases/:databaseId/tables', component: BrowseDatabaseComponent },
            { path: 'reports', component: ReportsComponent },
        ]),
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatSnackBarModule,
        ModalModule.forRoot()
    ],
    providers: [
        { provide: APP_ID, useValue: 'ng-cli-universal' },
        { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi())
    ]
})
export class AppModule { }
