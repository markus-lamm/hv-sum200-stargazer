# Stargazer

*This is a short explanation of the project. For the full documentation including screenshots, please see the files in the `Docs` folder.*

## Introduction

The purpose of developing an application focused on viewing and collecting astronomical phenomena is founded in the strong human desire to explore the vast beyond. The application seeks to tap into the fundamental human traits of curiosity and awe. Stargazer seeks to sate those desires by giving the user a way to experience the advances in space faring and humankind´s exploration of the observable universe through their phone.

## Technical Features

The application has a hybrid data source structure where an internet connection allows the user to partake in new imagery and their contents. While an offline mode still allows the user to explore their existing collection. This is achieved by segregating the data from the different data sources to designated pages within the application. 

The “Daily” and “Gallery” pages and their underlying pages consist of data fetched from a publicly available API service provided by the National Aeronautics and Space Administration (NASA). Data is directly fetched from their vast archived storage through the Astronomy Picture of the Day (APOD) API. On the other hand, data in the “Collection” page and underlying pages are fetched from a local SQLite database. 