from build_training_set import create_training_set_csv
from build_test_set import create_test_set
from train_knn import create_knn_classifier
from knn_classifier import classify_test_image
from knn_classifier import create_facemask
import os
import argparse


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("num_training_images", help="Number of images you want to use for your training set", type=int)
    parser.add_argument("test_image_name", help="The test image that you want to generate a facemask for")
    parser.add_argument("--knn", help="Set a custom value for k (default 24)", type=int)
    parser.add_argument("--cpus", help="Number of images you want to use for your training set (default is 4 CPUs)", type=int)
    parser.add_argument("--overlap_pixels", help="The number of pixels to shift by when building the test set (default=7; no overlap)", type=int)
    args = parser.parse_args()
    number_of_training_images = args.num_training_images  # Number of images to use for training
    test_image_name = args.test_image_name
    if args.cpus:
        number_of_cpus_for_knn = args.cpus
    else:
        number_of_cpus_for_knn = 4  # Set the number of CPUs to 4 by default
    if args.overlap_pixels:
        overlap_increment = args.overlap_pixels
        overlap = str(args.overlap_pixels) + "px-overlap"
    else:
        overlap_increment = 7  # Set the number of pixels to overlap if no custom number is given
        overlap = "no-overlap"
    if args.knn:
        k_value = args.knn
    else:
        k_value = 24


    # Setting variables for the images we want to classify
    classifier_csv = "/root/datascience/working_directory/training/classifier_"\
                     + str(number_of_training_images) + "_" + str(overlap_increment) + ".csv"
    knn_trained_classifier = "/root/datascience/working_directory/training/knn_trained_classifier-"\
                             + str(number_of_training_images) + "_k" \
                             + str(k_value) + "_" + str(number_of_cpus_for_knn) + "cpu_"\
                             + str(overlap_increment) + "px.pkl"  # Name our saved classifier so the filename has the
                            # number of training images, k value, number of cpus, and overlap increment

    # Setting variables for the test image we want to create a facemask for
    test_image_csv = "/root/datascience/working_directory/test_image_csv/" + test_image_name.split(".")[0] + ".csv"

    # Name our facemask file
    facemask_file = "/root/datascience/working_directory/facemask/" \
                    + test_image_name.split(".")[0] + "_" + str(number_of_training_images) + "_k" \
                    + str(k_value) + "_" + overlap + "px.jpg"  # If your file is named im02002.jpg and you
    # used 100 training images and a k value of 4 to create the knn algorithm and no overlapping pixels in the training set,
    # the image name would be: im02002_100_k4_no-overlap.jpg

    # Create the training set and train our k-NN algorithm if the classifier doesn't exist yet:
    if os.path.isfile(knn_trained_classifier):
        print("We already have a k-NN classifier for this set")
    else:
        create_training_set_csv(number_of_training_images, classifier_csv, overlap_increment)
        create_knn_classifier(classifier_csv, knn_trained_classifier, k_value, number_of_cpus_for_knn)

    # Create the test set for our image if it doesn't already exist
    if os.path.isfile(test_image_csv):
        print("We already have a CSV file built for this test image")
    else:
        create_test_set(test_image_name)

    # Classify our test images
    test_image_dataframe = classify_test_image(test_image_name, knn_trained_classifier)

    # Create our facemask
    create_facemask(test_image_dataframe, test_image_name, facemask_file)


if __name__ == "__main__":
    main()