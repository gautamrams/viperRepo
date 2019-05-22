import pandas as pd
from sklearn.externals import joblib
import time
from datetime import datetime
from PIL import Image


# Function to classify the pixels in our test data set
def classify_test_image(test_image_name, saved_knn_classifier):
    column_names = ["rbgb_" + str(number) for number in range(1, 148)]
    column_names.append("center pixel")
    test_image_csv = "/root/datascience/working_directory/test_image_csv/" + test_image_name.split(".")[0] + ".csv"
    training_data = pd.read_csv(test_image_csv, names=column_names)  # Create the dataframe
    classify_data = training_data.iloc[:, :-1].values  # Extract all the RGB values and leave off the center pixel location
    classifier = joblib.load(saved_knn_classifier)  # Load our k-NN classifier
    start_classifier = time.time()
    classifier_start_time = datetime.now()
    print("\nStarting the classification process at " + classifier_start_time.strftime("%I:%M:%S %p"))
    y_pred = classifier.predict(classify_data)  # Function to predict the classifier for a given 7x7 RGB grid
    end_classifier = time.time()
    print("-> " + (str(round((end_classifier - start_classifier), 2)) + " seconds to classify the training data with k-NN <-\n"))
    training_data['classifier'] = ""
    for classifier in range(0, len(y_pred)):  # Append the classifier to our dataframe
        training_data.at[classifier, 'classifier'] = y_pred[classifier]
    return training_data


# Function to create the facemask iamge
def create_facemask(dataframe, image_to_facemask, facemask_file):
    start_facemask_time = time.time()
    test_image = Image.open("/root/datascience/original/test/" + image_to_facemask)
    source_image_width, source_image_height = test_image.size  # Get the size of the test image we are facemasking
    my_facemask_image = Image.new('RGB', (source_image_width, source_image_height), (255, 255, 255))  # Create a white image
    facemask_image = my_facemask_image.load()

    for i, row in dataframe.loc[dataframe['classifier'] == 1].iterrows():  # Find all the rows in our dataframe that have a 1 as its classifier (skin)
        center_pixel_x = int(str(dataframe.loc[i, 'center pixel']).split("|")[0])
        center_pixel_y = int(str(dataframe.loc[i, 'center pixel']).split("|")[1])
        pixel_rgb = tuple([int(rgb) for rgb in dataframe.iloc[i, 72:75]])  # The center pixel RGB color is located in columns 72 - 75
        facemask_image[center_pixel_x, center_pixel_y] = pixel_rgb  # Set the pixel in the facemask to be the center pixel's RGB
    my_facemask_image.save(facemask_file)
    end_facemask_time = time.time()
    print("-> " + (str(round((end_facemask_time - start_facemask_time), 2)) + " seconds to generate the facemask <-\n"))
    print("Created your facemask. Check it out -> " + facemask_file)
